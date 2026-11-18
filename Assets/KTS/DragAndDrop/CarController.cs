using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelJoint2D _lWheel;
    [SerializeField] private WheelJoint2D _rWheel;
    public void Update()
    {
        float input = (Keyboard.current.dKey.isPressed ? -1 : 1) - (Keyboard.current.aKey.isPressed ? -1 : 1);
        input *= -1;
        JointMotor2D lmotor = _lWheel.motor;
        lmotor.motorSpeed = input * 500;
        _lWheel.motor = lmotor;
        JointMotor2D rmotor = _rWheel.motor;
        rmotor.motorSpeed = input * 500;
        _rWheel.motor = rmotor;
    }
}
