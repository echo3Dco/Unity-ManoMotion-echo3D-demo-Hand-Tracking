using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
namespace ManoMotion.HowToUse
{
    public enum InstructionsState
    {
        ShouldSee = -1,
        Seen = 1,
    }

    public class HowToInstructor : MonoBehaviour
    {

        bool hasPlayerSeenInstructions;
        string instructionsKey = "Instructions";

        public HowToAnimations triggerAnimations;
        [SerializeField]
        Instruction[] instructions;

        Image instructionCardBackgroundImage;
        Text instructionCanvasText;
        Text instructionCardText;
        Text instructionCardTitle;

        [SerializeField]
        GameObject howToCanvas;

        [SerializeField]
        GameObject logoWithInstructions;

        bool hasGoneThroughInstructions;
        private int currentInstruction;

        public event Action OnHasSeenAllInstructions = delegate { };
        public event Action OnHasSkippedInstructions = delegate { };

        [SerializeField]
        GameObject instructionCardObject;

        [SerializeField]
        GameObject confirmationObject;

        [SerializeField]
        GameObject skipInstructionsObject;

        [SerializeField]
        GameObject canvasInstructionsObject;

        bool displayCard;

        /// <summary>
        /// If no value is stored in the PlayerPrefs we set a value and saves it.
        /// </summary>
        private void Awake()
        {
            if (!PlayerPrefs.HasKey(instructionsKey))
            {
                PlayerPrefs.SetInt(instructionsKey, (int)InstructionsState.ShouldSee);
                PlayerPrefs.Save();
            }
        }

        private void Update()
        {
            HandleInstructionsHighlight();
        }

        public void DisplayFirstTimeInstructions()
        {
            RetrieveInstructionsHistory();
        }

        /// <summary>
        /// Check if instrucations have been seen or should be seen.
        /// </summary>
        void RetrieveInstructionsHistory()
        {
            hasPlayerSeenInstructions = PlayerPrefs.GetInt(instructionsKey) == (int)InstructionsState.Seen;

            if (hasPlayerSeenInstructions)
            {
                SkipInstructions();
            }

            else
            {
                InitializeHowtoInstructor();
                PlayerPrefs.SetInt(instructionsKey, (int)InstructionsState.Seen);
                PlayerPrefs.Save();
            }
        }

        public void InitializeHowtoInstructor()
        {
            hasGoneThroughInstructions = false;
            if (!howToCanvas)
            {
                string canvasName = "HowToInstructionCanvas";
                try
                {
                    howToCanvas = GameObject.Find(canvasName);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }

            }
            howToCanvas.SetActive(!hasGoneThroughInstructions);

            if (!logoWithInstructions)
            {
                try
                {
                    string name = "LogoWithInstructions";
                    logoWithInstructions = GameObject.Find(name);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            if (!instructionCardBackgroundImage)
            {
                try
                {
                    string canvasObjectName = "InstructionsRunTime";
                    instructionCardBackgroundImage = GameObject.Find(canvasObjectName).GetComponent<Image>();

                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);

                }
            }

            if (!instructionCanvasText)
            {
                try
                {
                    string canvasObjectName = "InstructionObject";
                    instructionCanvasText = GameObject.Find(canvasObjectName).GetComponent<Text>();

                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);

                }
            }

            if (!instructionCardText)
            {
                try
                {
                    string cardTextName = "InstructionCardDescription";
                    instructionCardText = GameObject.Find(cardTextName).GetComponent<Text>();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

            if (!instructionCardTitle)
            {
                try
                {
                    string cardTitleName = "InstructionCardtitle";
                    instructionCardTitle = GameObject.Find(cardTitleName).GetComponent<Text>();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

            if (!triggerAnimations)
            {
                try
                {
                    triggerAnimations = this.GetComponent<HowToAnimations>();

                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            currentInstruction = 0;

            for (int i = 0; i < instructions.Length; i++)
            {
                instructions[i].DeclareInstructionNotSeen();
                instructions[i].StopResponding();
            }

            triggerAnimations.HighlightImagesUpToStep(0);
            displayCard = true;
            ShouldDisplayCard(displayCard);
            ShouldDisplayInstructions(!displayCard);
            PlaceTextOnCard();
            ShouldShowTriggerAnimationVisuals(!displayCard);

        }

        /// <summary>
        /// Progresses the with next instruction.
        /// </summary>
        private void ProgressWithNextInstruction()
        {

            displayCard = true;
            ShouldDisplayCard(displayCard);
            ShouldDisplayInstructions(!displayCard);
            PlaceTextOnCard();

        }
        /// <summary>
        /// Skips the instructions.
        /// </summary>
        public void SkipInstructions()
        {
            PlayerPrefs.SetInt(instructionsKey, (int)InstructionsState.Seen);
            PlayerPrefs.Save();
            CloseHowToInstructions();
            OnHasSkippedInstructions();
        }

        /// <summary>
        /// Confirms the instruction seen.
        /// </summary>
        public void ConfirmInstructionSeen()
        {
            Handheld.Vibrate();
            DehighlightConfirmation();
            instructions[currentInstruction].DeclareInstructionSeen();
            instructions[currentInstruction].StopResponding();

            if (AreAllInstructionFinished())
            {
                PlayerPrefs.SetInt(instructionsKey, (int)InstructionsState.Seen);
                PlayerPrefs.Save();
                CloseHowToInstructions();
                OnHasSeenAllInstructions();
            }

            if (currentInstruction < instructions.Length)
            {
                currentInstruction++;
                ProgressWithNextInstruction();

            }
        }
        /// <summary>
        /// Shoulds the display card.
        /// </summary>
        /// <param name="state">If set to <c>true</c> state.</param>
        void ShouldDisplayCard(bool state)
        {
            instructionCardObject.SetActive(state);
        }
        /// <summary>
        /// Shoulds the display instructions.
        /// </summary>
        /// <param name="state">If set to <c>true</c> state.</param>
        void ShouldDisplayInstructions(bool state)
        {
            logoWithInstructions.SetActive(state);
            instructionCardBackgroundImage.enabled = state;
            canvasInstructionsObject.SetActive(state);
            confirmationObject.SetActive(state);
            skipInstructionsObject.SetActive(state);
        }
        /// <summary>
        /// Places the text on card.
        /// </summary>
        public void PlaceTextOnCard()
        {
            if (currentInstruction < instructions.Length)
            {
                instructionCardText.text = instructions[currentInstruction].GetInstructionCardText();
                instructionCardTitle.text = instructions[currentInstruction].GetInstructionCardTitle();
            }

        }
        /// <summary>
        /// Illustrates the current instruction on canvas.
        /// </summary>
        void IllustrateCurrentInstructionOnCanvas()
        {
            HighlightInstructionsBackground();
            instructionCanvasText.text = instructions[currentInstruction].GetInstructionCanvasText();

        }
        /// <summary>
        /// Updates the current instruction step on canvas.
        /// </summary>
        /// <param name="text">Text.</param>
        public void UpdateCurrentInstructionStepOnCanvas(string text)
        {
            HighlightInstructionsBackground();
            instructionCanvasText.text = text;
        }

        bool shouldHighlight;
        public Color dimInstructionColor;
        /// <summary>
        /// Highlights the instructions background.
        /// </summary>
        void HighlightInstructionsBackground()
        {
            shouldHighlight = true;
            instructionCardBackgroundImage.color = Color.white;

        }
        /// <summary>
        /// Handles the instructions highlight.
        /// </summary>
        void HandleInstructionsHighlight()
        {
            if (shouldHighlight)
            {
                if (instructionCardBackgroundImage.color != dimInstructionColor)
                {
                    Color currentColor = Color.Lerp(instructionCardBackgroundImage.color, dimInstructionColor, Time.deltaTime);
                    instructionCardBackgroundImage.color = currentColor;
                }
                else
                {
                    shouldHighlight = false;
                }

            }

        }
        /// <summary>
        /// Ares all instruction finished.
        /// </summary>
        /// <returns><c>true</c>, if all instruction finished was ared, <c>false</c> otherwise.</returns>
        private bool AreAllInstructionFinished()
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                if (!instructions[i].HasBeenSeen())
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Shows the current instruction on canvas.
        /// </summary>
        public void ShowCurrentInstructionOnCanvas()
        {
            displayCard = false;
            ShouldDisplayCard(displayCard);
            ShouldDisplayInstructions(!displayCard);
            IllustrateCurrentInstructionOnCanvas();

            instructions[currentInstruction].GuideHowTo();

        }
        public GameObject confirmationHighlightObject;

        /// <summary>
        /// Highlights the confirmation.
        /// </summary>
        public void HighlightConfirmation()
        {
            if (!confirmationHighlightObject)
            {
                confirmationHighlightObject = confirmationObject.transform.GetChild(0).gameObject;
            }
            confirmationHighlightObject.SetActive(true);
            Handheld.Vibrate();
        }
        /// <summary>
        /// Dehighlights the confirmation.
        /// </summary>
        private void DehighlightConfirmation()
        {
            if (!confirmationHighlightObject)
            {
                confirmationHighlightObject = confirmationObject.transform.GetChild(0).gameObject;
            }
            confirmationHighlightObject.SetActive(false);
        }
        /// <summary>
        /// Closes the how to instructions.
        /// </summary>
        public void CloseHowToInstructions()
        {
            howToCanvas.SetActive(false);
        }

        /// <summary>
        /// Shoulds the show trigger animation visuals.
        /// </summary>
        /// <param name="state">If set to <c>true</c> state.</param>
        public void ShouldShowTriggerAnimationVisuals(bool state)
        {
            triggerAnimations.ShouldShowAnimationImage(state);
        }
    }
}

