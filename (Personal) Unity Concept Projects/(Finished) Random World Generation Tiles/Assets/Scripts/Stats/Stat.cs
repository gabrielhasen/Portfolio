using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

[Serializable]
public class Stat
{
    private float BaseValue;
    public float currentValue;
    protected readonly List<StatModifier> statModifiers;
    //stores reference to original list and prohibits changing it.
    //however if the original list is changed this list changes too.
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;
    //If at any moment we need a fresh empty list, we can call statModifiers.Clear().
    protected float lastBaseValue = float.MinValue;
    protected bool isDirty = true;
    protected float _value;

    //can be overwritten by other class if inherited
    public virtual float Value{
        get{
            if(isDirty || lastBaseValue != BaseValue){
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            currentValue = _value;
            return _value;
        }
    }

    //Stat Constructors
    public Stat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
    public Stat(float value) : this()
    {
        BaseValue = value;
        currentValue = BaseValue;
    }

    public virtual int AddModifier(StatModifier mod) { return AddModifier(mod, false); }
    public virtual int AddModifier(StatModifier mod, bool calcCurrentValue)
    {
        isDirty = true;
        statModifiers.Add(mod);

        //sort can Sort automaticall or you can add a function
        //into the Sort parameters and it will sort the list based
        //on your function.
        statModifiers.Sort(CompareModifierOrder);
        if(calcCurrentValue == true){ 
            float temp = Value; 
        }
        return 1;
    }
    protected int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if(a.Order < b.Order)
            return -1; //Should be before the second object
        else if(a.Order > b.Order)
            return 1; //Should be after the second object
        return 0;   //if(a.Order == b.Order)
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if(statModifiers.Remove(mod)){
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        //for loop is in reverse as when you remove an item from a list
        //it shifts all of the other items one place down.  So in reverse
        //it is more effecient as there isn't as many shifting of items.
        for(int i = statModifiers.Count - 1; i >= 0; i++)
        {
            if(statModifiers[i].Source == source){
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    protected float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;
        for(int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            if(mod.Type == StatModType.Flat){
                finalValue += mod.Value;
            }
            else if(mod.Type == StatModType.PercentMult){
                sumPercentAdd += mod.Value;
                //if at end of list or the next modifier isnt of this type
                if(i + 1 >= statModifiers.Count || statModifiers[i+1].Type != StatModType.PercentAdd){
                    finalValue *= + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if(mod.Type == StatModType.PercentMult){
                // *= 1 + mod.Value,  is taking a percentage
                // and them multiplying it by the original value 
                // this works with negative numnbers as well
                finalValue *= 1 + mod.Value;
            }
        }
        return (float)Math.Round(finalValue, 4);
    }
}
