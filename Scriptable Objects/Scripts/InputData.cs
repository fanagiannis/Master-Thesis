using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Input", menuName = "New ScriptableObject/Input")]
public class InputData : ScriptableObject
{
    [Header("Inputs")]
    public KeyCode SprintKey=KeyCode.LeftShift;
    public KeyCode InteractKey=KeyCode.F;
    public KeyCode CrouchKey=KeyCode.C;
    public KeyCode JumpKey=KeyCode.Space;
    public KeyCode LeanLeftKey=KeyCode.Q;
    public KeyCode LeanRightKey=KeyCode.E;
}
