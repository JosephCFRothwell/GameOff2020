#region Snippet Information and Use
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

using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Rothwell.Managers
{
    [RequireComponent(typeof(ManagerIO))]
    public class ManagerIO : MonoBehaviour
    {
        #region Class Variables
        private static GameObject _ioManagerObject;
        private static ManagerIO _ioManagerInstance;
        private string _dirPath, _logDirPath, _audioFilePath, _graphicsFilePath, _controlFilePath, _miscFilePath, _logFilePath, _logFileName;
        private string _writeText;
        private ManagerDebug _managerDebug;
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
                if (_ioManagerInstance != null) return _ioManagerInstance;
                _ioManagerInstance = FindObjectOfType<ManagerIO>();
                if (_ioManagerInstance != null) return _ioManagerInstance;
                _ioManagerInstance = new GameObject("ManagerIO", typeof(ManagerIO)).GetComponent<ManagerIO>();
                #endregion
                #endregion
                return _ioManagerInstance;
            }

            set 
            {
                _ioManagerInstance = value;
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

        private void Awake()
        {
            #region Instance Protection and Component Setup
            #region Code explanation
            /*
         * Protect this instance from destruction
         */
            #endregion
            #region Instance Protection and Component Setup Code
            _ioManagerObject = this.gameObject;
            DontDestroyMeOnLoad(_ioManagerObject);
            _managerDebug = GameObject.FindWithTag("ManagerDebug").GetComponent<ManagerDebug>();
            
            #endregion
            #endregion

            _dirPath = Application.dataPath + "/Config/";
            _logDirPath = Application.dataPath + "/Log/";
            _audioFilePath = "AudioConfig.config";
            _graphicsFilePath = "GraphicsConfig.config";
            _controlFilePath = "InputConfig.config";
            _miscFilePath = "MiscConfig.config";
            _logFileName = "Log.txt";
            _logFilePath = _logDirPath + _logFileName;
            _writeText = null;
        }


        public void IO_ReadWriteConfigFile(string audioOrGraphicsOrControlOrMisc, bool trueForWriteFalseForRead = false)
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
            string relevantFullPath;
            _writeText = null;

            switch (audioOrGraphicsOrControlOrMisc)
            {
                // (1)
                case "audio":
                {
                    // (4)
                    relevantFileName = _audioFilePath;
                    relevantFullPath = $"{_dirPath}{relevantFileName}";

                    // (5) and (5b)
                    var ifMuted = "{" + (ManagerAudio.AMI.savedVolumeMaster * 100);
                    var ifUnmuted = "{" + (ManagerAudio.AMI.maxVolumeMaster * 100);
                    // create a string for the other variables
                    var writeTextBase = "}\n{" + (ManagerAudio.AMI.realMaxVolumeMusic * 100) + "}\n{" + (ManagerAudio.AMI.realMaxVolumeSFX * 100) + "}\n{" + (ManagerAudio.AMI.realMaxVolumeVoice * 100) + "}";

                    //if we are muted, prefix writeTextBase with the muted option, and if unmuted then use current master volume version
                    if (ManagerAudio.AMI.audioIsMuted)
                    {
                        _writeText = $"{ifMuted}{writeTextBase}";
                    }
                    else if (ManagerAudio.AMI.audioIsMuted == false)
                    {
                        _writeText = $"{ifUnmuted}{writeTextBase}";
                    }
                    break;
                }
                case "graphics":
                    relevantFullPath = _graphicsFilePath;
                    break;
                case "control":
                    relevantFullPath = _controlFilePath;
                    break;
                case "misc":
                    relevantFullPath = _miscFilePath;
                    break;
                default:
                    _managerDebug.DebugMessage("Err: Invalid argument in IO_ReadWriteConfigFile() Part A");
                    IO_AppendToLogFile("Err: Invalid argument in IO_ReadWriteConfigFile() Part A");
                    return; // (1b)
            }

            // (6)
            if (!File.Exists(relevantFullPath))
            {
                _managerDebug.DebugMessage($"Err: {relevantFileName}: File does not exist, creating new file with current values.");
                IO_AppendToLogFile($"Err: {relevantFileName}: File does not exist, creating new file with current values.");
                Directory.CreateDirectory(_dirPath);
                File.WriteAllText(relevantFullPath, _writeText);
            }

            // (7)
            if (new FileInfo(relevantFullPath).Length == 0)
            {
                _managerDebug.DebugMessage($"Err: {relevantFileName}: File Empty, writing from current values.");
                File.Delete(relevantFullPath);
                File.WriteAllText(relevantFullPath, _writeText);
            }

            // (8)
            // (9)
            var deliminators = new[] { '{', '}', '\n' };
            if (trueForWriteFalseForRead == false)      // if in read mode
            {
                // (9a)
                string t = File.ReadAllText(relevantFullPath);
                // (9b)
                Regex regex = new Regex(@"^[0-9.{}\n]*$");
                if (!regex.IsMatch(t))
                {   // (9c)
                    _managerDebug.DebugMessage($"Err: {relevantFileName}: Invalid Character. Writing new from current values.");
                    IO_AppendToLogFile($"Err: {relevantFileName}: Invalid Character. Writing new from current values.");
                    File.Delete(relevantFullPath);
                    File.WriteAllText(relevantFullPath, _writeText);
                    t = File.ReadAllText(relevantFullPath);     // (9c(i))
                }

                // (9d)
                var variablesExtracted = t.Split(deliminators, StringSplitOptions.RemoveEmptyEntries);

                switch (audioOrGraphicsOrControlOrMisc)
                {
                    // (9e)
                    case "audio": // (9e(i))
                        ManagerAudio.AMI.maxVolumeMaster = ManagerAudio.AMI.Audio_Normalise0To100((float.Parse(variablesExtracted[0]) / 100));
                        ManagerAudio.AMI.realMaxVolumeMusic = ManagerAudio.AMI.Audio_Normalise0To100((float.Parse(variablesExtracted[1]) / 100));
                        ManagerAudio.AMI.realMaxVolumeSFX = ManagerAudio.AMI.Audio_Normalise0To100((float.Parse(variablesExtracted[2]) / 100));
                        ManagerAudio.AMI.realMaxVolumeVoice = ManagerAudio.AMI.Audio_Normalise0To100((float.Parse(variablesExtracted[3]) / 100));
                        break;
                    case "graphics": // what are we reading from graphics?
                        break;
                    case "control": // what are we reading from control?
                        break;
                    case "misc": // what are we reading from misc?
                        break;
                    default:
                        IO_AppendToLogFile("Err: Invalid argument in IO_ReadWriteConfigFile() Part B");
                        _managerDebug.DebugMessage("Err: Invalid argument in IO_ReadWriteConfigFile() Part B"); 
                        return; // (9f)
                } 
            }
            // (10)
            if (!trueForWriteFalseForRead) return; // (10a)
            File.Delete(relevantFullPath);
            File.WriteAllText(relevantFullPath, _writeText);
            #endregion
            #endregion

        }

        public void IO_AppendToLogFile(string outputString)
        {
            var datetime = DateTime.Now;
            var datetimeNormalised = datetime.ToString(CultureInfo.InvariantCulture);
            if (!File.Exists(_logFilePath))
            {
                _managerDebug.DebugMessage($"Err: {_logFilePath}: File does not exist, creating new log file.");
                Directory.CreateDirectory(_logDirPath);
                File.AppendAllText(_logFilePath, $"[{datetimeNormalised}] Created New Log File{Environment.NewLine}");
            }

            File.AppendAllText(_logFilePath, $"[{datetimeNormalised}] {outputString}{Environment.NewLine}");

        }
    }
}
