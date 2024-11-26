using System.Collections.Generic;

[System.Serializable]
public class LSystemRule
{
    public string axiom;
    public Dictionary<char, string> productions;

    public LSystemRule(string axiom, Dictionary<char, string> productions)
    {
        this.axiom = axiom;
        this.productions = productions;
    }
}
