using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instruction : MonoBehaviour
{
    protected string _instructionName;
    protected string _howToInstruction;
    protected string _confirmationInstruction;
    protected string _cardText;
    protected int _instructionID;
    protected bool _instructionSeen;
    protected bool _instructionPerformed;
    protected int _instructionSteps;
    protected int _currentInstructionStep;
    protected string[] _stepInstructions;
    protected bool _shouldRespondToUserInput;

    /// <summary>
    /// Gets all step instructions.
    /// </summary>
    /// <returns>The all step instructions.</returns>
    virtual public string[] GetAllStepInstructions()
    {
        return _stepInstructions;
    }
    /// <summary>
    /// Gets the step instructions.
    /// </summary>
    /// <returns>The step instructions.</returns>
    /// <param name="step">Step.</param>
    virtual public string GetStepInstructions(int step)
    {
        return _stepInstructions[step];

    }
    /// <summary>
    /// Gets the instruction canvas text.
    /// </summary>
    /// <returns>The instruction canvas text.</returns>
    virtual public string GetInstructionCanvasText()
    {
        return _howToInstruction;
    }
    /// <summary>
    /// Gets the instruction card text.
    /// </summary>
    /// <returns>The instruction card text.</returns>
    virtual public string GetInstructionCardText()
    {
        return _cardText;
    }
    /// <summary>
    /// Gets the instruction identifier.
    /// </summary>
    /// <returns>The instruction identifier.</returns>
    virtual public int GetInstructionID()
    {
        return _instructionID;
    }
    /// <summary>
    /// Gets the instruction card title.
    /// </summary>
    /// <returns>The instruction card title.</returns>
    virtual public string GetInstructionCardTitle()
    {
        return _instructionName;
    }

    /// <summary>
    /// Hases the been seen.
    /// </summary>
    /// <returns><c>true</c>, if been seen was hased, <c>false</c> otherwise.</returns>
    virtual public bool HasBeenSeen()
    {
        return _instructionSeen;
    }

    /// <summary>
    /// Declares the instruction seen.
    /// </summary>
    virtual public void DeclareInstructionSeen()
    {
        _instructionSeen = true;
    }
    /// <summary>
    /// Declares the instruction not seen.
    /// </summary>
    virtual public void DeclareInstructionNotSeen()
    {
        _instructionSeen = false;
    }

    /// <summary>
    /// Sets the instruction as performed.
    /// </summary>
    virtual public void SetInstructionAsPerformed()
    {
        _instructionPerformed = true;
    }
    /// <summary>
    /// Progresses the with instruction step.
    /// </summary>
    virtual public void ProgressWithInstructionStep()
    {

    }
    /// <summary>
    /// Instructions the behavior.
    /// </summary>
    virtual public void InstructionBehavior()
    {

    }
    /// <summary>
    /// Guides the how to.
    /// </summary>
    virtual public void GuideHowTo()
    {
        this._shouldRespondToUserInput = true;

    }

    /// <summary>
    /// Stops the responding.
    /// </summary>
    virtual public void StopResponding()
    {
        this._shouldRespondToUserInput = false;

    }
}
