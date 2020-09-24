using System;

public enum statRangeType
{
    Static,
    Level
}

[Serializable]
public class StatRange : Stat
{
    private float BaseMaxValue;
    public float currentMaxValue;
    private bool isNeedingReset = false;
    private float _baseValue;
    private statRangeType BaseRangeType;

    public virtual float ValueMax{
        get{ 
            if(isNeedingReset){
                currentValue = _baseValue;
                isNeedingReset = false;
            }
            currentMaxValue = _baseValue;
            return BaseMaxValue; 
        }
    }

    public StatRange(float value, float maxValue, statRangeType rangeType) : base(value)
    {
        BaseMaxValue = maxValue;
        BaseRangeType = rangeType;
        currentMaxValue = BaseMaxValue;
    }

    //Value and MaxValue will set default statRangeType to static
    public StatRange(float value, float maxValue) : this(value, maxValue, statRangeType.Static) {}

    /*
        Checks value and maxValue
        return 0 = modifier added
        return 1 = larger than or equal to baseMaxValue
        return 2 = less than  or equal to 0 
    */
    public override int AddModifier(StatModifier mod) { return AddModifier(mod, false); }
    public override int AddModifier(StatModifier mod, bool calcCurrentValue)
    {
        base.AddModifier(mod, calcCurrentValue);
        float value = Value;
        if(value >= BaseMaxValue){
            RangeChoice();
            return 1;
        }
        else if(value <= 0){
            RangeChoice();
            return 2;
        }
        return 0;
    }

    private void RangeChoice()
    {
        if(BaseRangeType == statRangeType.Static){
            RangeTypeStaticReset();
        }
        else if(BaseRangeType == statRangeType.Level){
            RangeTypeLevelReset();
        } 
        else{
            System.Console.WriteLine("ERROR: CLASS: StatRange Function: RangeChoice");
        }
    }

    private void RangeTypeStaticReset()
    {
        isNeedingReset = true;
        _baseValue = BaseMaxValue;
    }

    private void RangeTypeLevelReset()
    {
        isNeedingReset = true;
        _baseValue = 0;
        BaseMaxValue += 5;
    }
}