// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using UnityEngine;

#if UNITY_WSA || UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Text;
using UnityEngine.Windows.Speech;
#endif


/// <summary>
/// Singleton class that implements the DictationRecognizer to convert the user's speech to text.
/// The DictationRecognizer exposes dictation functionality and supports registering and listening for hypothesis and phrase completed events.
/// </summary>
public class DictationInputManager : MonoBehaviour
{
#if UNITY_WSA || UNITY_STANDALONE_WIN || UNITY_EDITOR
    /// <summary>
    /// Caches the text currently being displayed in dictation display text.
    /// </summary>
    private static StringBuilder textSoFar;

    /// <summary>
    /// <remarks>Using an empty string specifies the default microphone.</remarks>
    /// </summary>
    private static readonly string DeviceName = string.Empty;

    /// <summary>
    /// The device audio sampling rate.
    /// <remarks>Set by UnityEngine.Microphone.<see cref="Microphone.GetDeviceCaps"/></remarks>
    /// </summary>
    private static int samplingRate;

    /// <summary>
    /// Is the Dictation Manager currently running?
    /// </summary>
    public static bool IsListening { get; private set; }

    /// <summary>
    /// String result of the current dictation.
    /// </summary>
    private static string dictationResult;

    /// <summary>
    /// Audio clip of the last dictation session.
    /// </summary>
    private static AudioClip dictationAudioClip;

    private static DictationRecognizer dictationRecognizer;

    private static bool isTransitioning;
    private static bool hasFailed;
    public static DictationInputManager Instance { get; private set; }
#endif

    #region Unity Methods

#if UNITY_WSA || UNITY_STANDALONE_WIN || UNITY_EDITOR
    private void Awake()
    {

        Debug.Log("DictationInputManager Awake");

        dictationResult = string.Empty;
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
        IsListening = false;
        isTransitioning = false;
        // Query the maximum frequency of the default microphone.
        int minSamplingRate; // Not used.
        Microphone.GetDeviceCaps(DeviceName, out minSamplingRate, out samplingRate);
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);




    }



    private void LateUpdate()
    {
        if (IsListening && !Microphone.IsRecording(DeviceName) && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            // If the microphone stops as a result of timing out, make sure to manually stop the dictation recognizer.
            Debug.Log("Dictation has timed out. Stopping dictation recognizer.");
            StartCoroutine(StopRecording());
        }

        if (!hasFailed && dictationRecognizer.Status == SpeechSystemStatus.Failed)
        {
            Debug.Log("Dictation recognizer has failed!");
            hasFailed = true;
        }
    }

    void OnDestroy()
    {
        dictationRecognizer.Stop();
        dictationRecognizer.Dispose();
    }
    void OnApplicationQuit()
    {
        dictationRecognizer.Stop();
        dictationRecognizer.Dispose();
    }
#endif

    #endregion // Unity Methods

    /// <summary>
    /// Turns on the dictation recognizer and begins recording audio from the default microphone.
    /// </summary>
    /// <param name="initialSilenceTimeout">The time length in seconds before dictation recognizer session ends due to lack of audio input in case there was no audio heard in the current session.</param>
    /// <param name="autoSilenceTimeout">The time length in seconds before dictation recognizer session ends due to lack of audio input.</param>
    /// <param name="recordingTime">Length in seconds for the manager to listen.</param>
    /// <returns></returns>
    public static IEnumerator StartRecording(float initialSilenceTimeout = 10f, float autoSilenceTimeout = 10f, int recordingTime = 60)
    {
        Debug.Log("Starting dictation recognizer.");
#if UNITY_WSA || UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (IsListening)
        {
            Debug.LogWarning("Unable to start recording reason: IS_LISTENING");
            yield break;
        }

        while (isTransitioning)
        {
            //log only if developement build
            Debug.Assert(Debug.isDebugBuild, "DictationInputManager is transitioning between states. Please wait for transition to complete.");
            yield return null;
        }

        IsListening = true;
        isTransitioning = true;

        if (PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
        {

            PhraseRecognitionSystem.Shutdown();
        }

        while (PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
        {
            Debug.Assert(Debug.isDebugBuild, "PhraseRecognitionSystem IS RUNNING!");
            yield return null;
        }

        dictationRecognizer.InitialSilenceTimeoutSeconds = initialSilenceTimeout;
        dictationRecognizer.AutoSilenceTimeoutSeconds = autoSilenceTimeout;
        dictationRecognizer.Start();

        while (dictationRecognizer.Status == SpeechSystemStatus.Failed)
        {
            Debug.Assert(Debug.isDebugBuild, "Dictation recognizer failed to start!");
            yield break;
        }

        while (dictationRecognizer.Status == SpeechSystemStatus.Stopped)
        {
            Debug.Assert(Debug.isDebugBuild, "Dictation recognizer is stopped. Waiting...");
            yield return null;
        }

        // Start recording from the microphone.
        dictationAudioClip = Microphone.Start(DeviceName, false, recordingTime, samplingRate);

        textSoFar = new StringBuilder();
        isTransitioning = false;
#else
            return null;
#endif
    }

    /// <summary>
    /// Ends the recording session.
    /// </summary>
    public static IEnumerator StopRecording()
    {
        Debug.Log("Stopping dictation recognizer.");
#if UNITY_WSA || UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (!IsListening || isTransitioning)
        {
            Debug.Assert(Debug.isDebugBuild, "Unable to stop recording");
            yield break;
        }

        IsListening = false;
        isTransitioning = true;

        Microphone.End(DeviceName);

        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
        }

        while (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            yield return null;
        }

        PhraseRecognitionSystem.Restart();
        isTransitioning = false;
#else
            return null;
#endif
    }

    #region Dictation Recognizer Callbacks
#if UNITY_WSA || UNITY_STANDALONE_WIN || UNITY_EDITOR

    /// <summary>
    /// This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
    /// </summary>
    /// <param name="text">The currently hypothesized recognition.</param>
    private static void DictationRecognizer_DictationHypothesis(string text)
    {
        // We don't want to append to textSoFar yet, because the hypothesis may have changed on the next event.
        Debug.Log("Hypothesis: " + text);
        dictationResult = textSoFar.ToString() + " " + text + "...";
        VoiceInputHandler.Instance.RaiseDictationHypothesis(dictationResult);

    }

    /// <summary>
    /// This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
    /// </summary>
    /// <param name="text">The text that was heard by the recognizer.</param>
    /// <param name="confidence">A representation of how confident (rejected, low, medium, high) the recognizer is of this recognition.</param>
    private static void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("Dictation result: " + text);

        textSoFar.Append(text);
        dictationResult = textSoFar.ToString();
        //  Debug.LogFormat("Dictation result: {0}", dictationResult);
        VoiceInputHandler.Instance.RaiseDictationResult(text);
    }

    /// <summary>
    /// This event is fired when the recognizer stops, whether from StartRecording() being called, a timeout occurring, or some other error.
    /// Typically, this will simply return "Complete". In this case, we check to see if the recognizer timed out.
    /// </summary>
    /// <param name="cause">An enumerated reason for the session completing.</param>
    private static void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        IsListening = false;
        // If Timeout occurs, the user has been silent for too long.
        if (cause == DictationCompletionCause.TimeoutExceeded)
        {
            Microphone.End(DeviceName);
            dictationResult = "Dictation has timed out. Please try again.";

        }

        if (dictationResult == string.Empty || cause != DictationCompletionCause.Complete)
        {
            dictationResult = "Dictation restarting: " + cause.ToString();
            Instance.StartCoroutine(StartRecording());
        }

        VoiceInputHandler.Instance.RaiseDictationComplete(cause != DictationCompletionCause.Complete, DictationCompletionCause.Complete.ToString(), dictationResult, dictationAudioClip);
        textSoFar = null;
        dictationResult = string.Empty;
    }

    /// <summary>
    /// This event is fired when an error occurs.
    /// </summary>
    /// <param name="error">The string representation of the error reason.</param>
    /// <param name="hresult">The int representation of the hresult.</param>
    private static void DictationRecognizer_DictationError(string error, int hresult)
    {
        dictationResult = error + "\nHRESULT: " + hresult.ToString();

        VoiceInputHandler.Instance.RaiseDictationError(dictationResult);
        textSoFar = null;
        dictationResult = string.Empty;
    }
#endif
    #endregion // Dictation Recognizer Callbacks


}

