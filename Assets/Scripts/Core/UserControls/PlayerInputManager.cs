using History;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        public CanvasGroup historyLogCanvasGroup;
        public static bool isCutScenePlaying = false;
        private PlayerInput input;
        private List<(InputAction action, Action<InputAction.CallbackContext> command)> actions = new List<(InputAction action, Action<InputAction.CallbackContext> command)> ();

        private void Awake()
        {
            input = GetComponent<PlayerInput> ();
            
            InitializeActions();
        }

        private void InitializeActions()
        {
            actions.Add((input.actions["Next"], OnNext));
            actions.Add((input.actions["HistoryBack"], OnHistoryBack));
            actions.Add((input.actions["HistoryForward"], OnHistoryForward));
            actions.Add((input.actions["HistoryLogs"], OnHistoryToggleLog));
            actions.Add((input.actions["HistoryLogs_Scroll"], OnHistoryToggleLog_Scroll));
        }

        private void OnEnable()
        {
            foreach (var inputAction in actions)
                inputAction.action.performed += inputAction.command;
        }

        private void OnDisable()
        {
            foreach (var inputAction in actions)
                inputAction.action.performed -= inputAction.command;
        }

        public void OnNext(InputAction.CallbackContext c)
        {
            if (historyLogCanvasGroup.alpha.Equals(0) && !isCutScenePlaying)
            {
                DialogueSystem.instance.OnUserPrompt_Next();
            }
        }

        public void OnHistoryBack(InputAction.CallbackContext c)
        {
            HistoryManager.instance.GoBack();
        }

        public void OnHistoryForward(InputAction.CallbackContext c)
        {
            HistoryManager.instance.GoForward();
        }

        public void OnHistoryToggleLog(InputAction.CallbackContext c)
        {
            var logs = HistoryManager.instance.logManager;

            if (!logs.isOpen)
                logs.Open();
            else
                logs.Close();
            
        }
        public void OnHistoryToggleLog_Scroll(InputAction.CallbackContext c)
        {
            var logs = HistoryManager.instance.logManager;

            if (!logs.isOpen)
                logs.Open();
        }
    }
}