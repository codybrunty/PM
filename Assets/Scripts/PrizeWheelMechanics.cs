using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeWheelMechanics : MonoBehaviour{

    public int targetNumber=0;
    //float rotationSpeed = 0f;
    private readonly List<int> _weightedList = new List<int>();
    private bool _spinning;
    private float _anglePerItem;
    public int spinsMin=3;
    public int spinsMax=4;
    public float SpinDuration = 5;
    [SerializeField] GameObject soundTriggers = default;
    public AnimationCurve wheelSpin=default;

    public class WeightedValue {
        public int Value;
        public int Weight;

        public WeightedValue(int value, int weight) {
            Value = value;
            Weight = weight;
        }
    }
    public List<WeightedValue> PricesWithWeights = new List<WeightedValue>{
    //               Value | Weight TODO: Make sure these sum up to 100
    new WeightedValue(0,        17),
    new WeightedValue(1,        17),
    new WeightedValue(2,        16),
    new WeightedValue(3,        17),
    new WeightedValue(4,        16),
    new WeightedValue(5,        17),
    };

    private void Start() {
        _spinning = false;
        _anglePerItem = 360f / PricesWithWeights.Count;
        
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(0f, 360f));

        // first fill the randomResults accordingly to the given wheights
        foreach (var kvp in PricesWithWeights) {
            // add kvp.Key to the list kvp.value times
            for (var i = 0; i < kvp.Weight; i++) {
                _weightedList.Add(kvp.Value);
            }
        }
        
    }

    public int GetRandomNumber() {
        var randomIndex = UnityEngine.Random.Range(0, _weightedList.Count);
        Debug.Log(randomIndex);
        // get the according value
        return _weightedList[randomIndex];
    }

    private void TurnSoundOn() {
        soundTriggers.SetActive(true);
    }

    public void SpinWheel() {

        if (_spinning) return;

        TurnSoundOn();

        var randomTime = UnityEngine.Random.Range(spinsMin, spinsMax+1);
        var itemNumber = targetNumber;
        var currentAngle = transform.eulerAngles.z;
        var itemIndex = PricesWithWeights.FindIndex(w => w.Value == itemNumber);
        var itemNumberAngle = itemIndex * _anglePerItem;

        while (currentAngle >= 360) {
            currentAngle -= 360;
        }
        while (currentAngle < 0) {
            currentAngle += 360;
        }

        var targetAngle = -(itemNumberAngle + 360f * randomTime);
        //Debug.Log($"Will spin {randomTime } times before ending at {itemNumber} with an angle of {itemNumberAngle}", this);
        //Debug.Log($"The odds for this were {PricesWithWeights[itemIndex].Weight / 100f:P} !");
        StartCoroutine(SpinTheWheel(currentAngle, targetAngle, SpinDuration, itemNumber));
    }


    private IEnumerator SpinTheWheel(float fromAngle, float toAngle, float withinSeconds, int result) {
        _spinning = true;
        var passedTime = 0f;

        //Debug.LogWarning(fromAngle);
        //Debug.LogWarning(toAngle);


        while (passedTime < withinSeconds) {

            var lerpFactor = wheelSpin.Evaluate(passedTime / withinSeconds);

            //var lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, passedTime / withinSeconds)));
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(fromAngle, toAngle, lerpFactor));


            passedTime += Time.deltaTime;
            yield return null;
        }

        transform.localEulerAngles = new Vector3(0.0f, 0.0f, toAngle);
        _spinning = false;
        Debug.Log("Prize wheel moved to position 5");
    }
}
