﻿#region Snippet Information and Use
/* Creator Information
 *
 * Script Name: ManagerIO
 * Author: Joseph CF Rothwell
*/

/* Steps for use
 *
 * 1) Do...
 */
#endregion

using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine;

public class ManagerIO : MonoBehaviour
{
    #region Class Variables
    private static GameObject ioManagerObject;
    private static ManagerIO ioManagerInstance;
    private string dirPath, logDirPath, audioFilePath, graphicsFilePath, controlFilePath, miscFilePath, logFilePath, logFileName;
    private string writeText;
    #endregion

    public static ManagerIO IOMI
    {
            #region Existence check
            #region Code explanation
            /* 
             * Find the ManagerIO gameobject and set the variable. 
             * If there is no such game object, create one with this script and set variables.

             */
            #endregion
            #region Existence check code
        get
        {
            if (ioManagerInstance == null)
            {
                ioManagerInstance = FindObjectOfType<ManagerIO>();
                if (ioManagerInstance == null)
                {
                    ioManagerInstance = new GameObject("ManagerIO", typeof(ManagerIO)).GetComponent<ManagerIO>();
                }
            }
            #endregion
            #endregion
            return ioManagerInstance;
        }

        set 
        {
            ioManagerInstance = value;
        }
    }


    public static void DontDestroyMeOnLoad(GameObject thisObject)
    {
        // This protects this, and objects above it (eg Managers gameobject), from being destroyed on load
        // This also means don't need to protect other manager classes?
        Transform parentTransform = thisObject.transform;

        // If this object doesn't have a parent then its the root transform.
        while (parentTransform.parent != null)
        {
            // Keep going up the chain.
            parentTransform = parentTransform.parent;
        }
        GameObject.DontDestroyOnLoad(parentTransform.gameObject);
    }

    void Awake()
    {
        #region Instance Protection and Component Setup
        #region Code explanation
        /*
         * Protect this instance from destruction
         */
        #endregion
        #region Instance Protection and Component Setup Code
        ioManagerObject = this.gameObject;
        DontDestroyMeOnLoad(ioManagerObject);
        #endregion
        #endregion

        dirPath = Application.dataPath + "/Config/";
        logDirPath = Application.dataPath + "/Log/";
        audioFilePath = "AudioConfig.config";
        graphicsFilePath = "GraphicsConfig.config";
        controlFilePath = "InputConfig.config";
        miscFilePath = "MiscConfig.config";
        logFileName = "Log.txt";
        logFilePath = logDirPath + logFileName;
        writeText = null;
    }


    public void IO_ReadWriteConfigFile(string audio_or_graphics_or_control_or_misc, bool trueForWriteFalseForRead = false)
    {
        #region IO ReadWriteConfigFile(str, bool)
        #region Code Explanation
        /*
         * General method for Config File IO. 
         * 
         * The steps here are labelled in the code with // (#a(i)) except for 2, 3
         * 
         * 1) We work out from the arguments what type of config file to use ("audio", "graphics", "control", "misc")
         *          1b) if this is an incorrect option, we debug error and return
         * 2) If the second argument is not included, it defaults to false
         * 3) The second argument tells us write if true, read if false
         * 4) We create a path variable depending on arg0 input (example in Audio)
         * 5) We take some actions depending on what config file we are dealing with
         *          5b) eg audio, we check if we are muted and use that to create the text to write if it is needed
         * 6) We check if the file actually exists, if not we create and use writeText to populate it
         * 7) We check if the file exists but is empty, if so we remove it and create a new one as in (6)
         * 8) We move on to the read/write specific code
         * 9) Read mode code
         *          9a) We set out our custom deliminators and read in the file
         *          9b) Regex to ensure it is only allowed characters
         *          9c) if there is an invalid character, we remove the file and create as in (6)
         *                  9c(i)) in this case, also read in the new text file so that variable is good text
         *          9d) we extract the deliminators so the elements of array only have variables
         *          9e) Depending on what config file we are dealing with, we take file-specific read actions
         *                  9e(i)) eg audio, we set the volumes in ManagerAudio
         *          9f) We again protect against incorrect filename, redundant but just in case
         * 10) Write mode code
         *          10a) we just delete the config file and write a new one with writeText
         */
        #endregion
        #region Code

        string relevantFileName = null;
        string relevantFullPath = null;
        writeText = null;

        // (1)
        if (audio_or_graphics_or_control_or_misc == "audio")
        {
            // (4)
            relevantFileName = audioFilePath;
            relevantFullPath = dirPath + relevantFileName;

            // (5) and (5b)
            string ifMuted = "{" + (ManagerAudio.AMI.savedVolumeMaster * 100);
            string ifUnmuted = "{" + (ManagerAudio.AMI.maxVolumeMaster * 100);
            // create a string for the other variables
            string writeTextBase = "}\n{" + (ManagerAudio.AMI.realMaxVolumeMusic * 100) + "}\n{" + (ManagerAudio.AMI.realMaxVolumeSFX * 100) + "}\n{" + (ManagerAudio.AMI.realMaxVolumeVoice * 100) + "}";

            //if we are muted, prefix writeTextBase with the muted option, and if unmuted then use current master volume version
            if (ManagerAudio.AMI.audioIsMuted == true)
            {
                writeText = ifMuted + writeTextBase;
            }
            else if (ManagerAudio.AMI.audioIsMuted == false)
            {
                writeText = ifUnmuted + writeTextBase;
            }
        }
        else if (audio_or_graphics_or_control_or_misc == "graphics")
        {
            relevantFullPath = graphicsFilePath;
        }
        else if (audio_or_graphics_or_control_or_misc == "control")
        {
            relevantFullPath = controlFilePath;
        }
        else if (audio_or_graphics_or_control_or_misc == "misc")
        {
            relevantFullPath = miscFilePath;
        }
        else 
        {   
            Debug.LogError("Invalid argument in IO_ReadWriteConfigFile() Part A");
            IO_AppendToLogFile("Invalid argument in IO_ReadWriteConfigFile() Part A");
            return; 
        } // (1b)

        // (6)
        if (!File.Exists(relevantFullPath))
        {
            Debug.Log("Err: " + relevantFileName + ": File does not exist, creating new file with current values.");
            IO_AppendToLogFile("Err: " + relevantFileName + ": File does not exist, creating new file with current values.");
            Directory.CreateDirectory(dirPath);
            File.WriteAllText(relevantFullPath, writeText);
        }

        // (7)
        if (new FileInfo(relevantFullPath).Length == 0)
        {
            Debug.Log("Err: " + relevantFileName + ": File Empty, writing from current values.");
            File.Delete(relevantFullPath);
            File.WriteAllText(relevantFullPath, writeText);
        }

        // (8)
        // (9)
        if (trueForWriteFalseForRead == false)      // if in read mode
        {
            // (9a)
            char[] deliminators = { '{', '}', '\n' };
            string t = File.ReadAllText(relevantFullPath);
            // (9b)
            Regex regex = new Regex(@"^[0-9.{}\n]*$");
            if (!regex.IsMatch(t))
            {   // (9c)
                Debug.Log("Err: " + relevantFileName + ": Invalid Character. Writing new from current values.");
                IO_AppendToLogFile("Err: " + relevantFileName + ": Invalid Character. Writing new from current values.");
                File.Delete(relevantFullPath);
                File.WriteAllText(relevantFullPath, writeText);
                t = File.ReadAllText(relevantFullPath);     // (9c(i))
            }

            // (9d)
            string[] variablesExtracted = t.Split(deliminators, System.StringSplitOptions.RemoveEmptyEntries);

            // (9e)
            if (audio_or_graphics_or_control_or_misc == "audio")
            {   // (9e(i))
                ManagerAudio.AMI.maxVolumeMaster = ManagerAudio.AMI.Audio_Normalise0To100((float.Parse(variablesExtracted[0]) / 100));
                ManagerAudio.AMI.realMaxVolumeMusic = ManagerAudio.AMI.Audio_Normalise0To100((float.Parse(variablesExtracted[1]) / 100));
                ManagerAudio.AMI.realMaxVolumeSFX = ManagerAudio.AMI.Audio_Normalise0To100((float.Parse(variablesExtracted[2]) / 100));
                ManagerAudio.AMI.realMaxVolumeVoice = ManagerAudio.AMI.Audio_Normalise0To100((float.Parse(variablesExtracted[3]) / 100));
            }
            else if (audio_or_graphics_or_control_or_misc == "graphics")
            {   // what are we reading from graphics?
            }
            else if (audio_or_graphics_or_control_or_misc == "control")
            {   // what are we reading from control?
            }
            else if (audio_or_graphics_or_control_or_misc == "misc")
            {   // what are we reading from misc?
            }
            else 
            {
                IO_AppendToLogFile("Invalid argument in IO_ReadWriteConfigFile() Part B");
                Debug.LogError("Invalid argument in IO_ReadWriteConfigFile() Part B"); 
                return; // (9f)
            } 
        }
        // (10)
        if (trueForWriteFalseForRead == true)       // if in write mode
        {   // (10a)
            File.Delete(relevantFullPath);
            File.WriteAllText(relevantFullPath, writeText);
        }
        #endregion
        #endregion

    }

    public void IO_AppendToLogFile(string outputString)
    {
        var datetime = DateTime.Now;
        string datetimeNormalised = datetime.ToString(CultureInfo.InvariantCulture);
        if (!File.Exists(logFilePath))
        {
            Debug.Log("Err: " + logFilePath + ": File does not exist, creating new log file.");
            Directory.CreateDirectory(logDirPath);
            File.AppendAllText(logFilePath, "[" + datetimeNormalised + "] " + "Created New Log File" + Environment.NewLine);
        }

        File.AppendAllText(logFilePath, "[" + datetimeNormalised + "] " + outputString + Environment.NewLine);

    }
}
