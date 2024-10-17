using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionSystem : MonoBehaviour
{
    private static ProductionSystem _instance;
    public static ProductionSystem Instance { get { return _instance; } }

    Dictionary<string, bool> conditions;
    public Dictionary<string, List<string>> conditionalDialogue;
    bool hasGiven = false;

    // Start is called before the first frame update
    void Start()
    {
        conditions = new Dictionary<string, bool>();
        conditionalDialogue = new Dictionary<string, List<string>>();

        conditions.Add("intro", true);
        conditions.Add("reintro", false);
        conditions.Add("rand1", false);
        conditions.Add("rand2", false);

        initDemoSystem();
    }

    void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasGiven)
        {
            print("we got here");
            hasGiven = true;
            if (conditions["rand1"] && conditions["rand2"])
            {
                print("condition12");
                DialogueManager.Instance.StartDialogue(conditionalDialogue["12"]);
            }
            else if (conditions["rand1"])
            {
                print("condition1");
                DialogueManager.Instance.StartDialogue(conditionalDialogue["1"]);
            }
            else if (conditions["rand2"])
            {
                print("condition2");
                DialogueManager.Instance.StartDialogue(conditionalDialogue["1"]);
            }
            else
            {
                print("condition0");
                DialogueManager.Instance.StartDialogue(conditionalDialogue["0"]);
            }
        }
    }

    private void initDemoSystem()
    {
        float randChance;
        string rand = "rand";
        bool chance;

        for (int i = 1; i <= 2; i++)
        {
            randChance = Random.Range(0f, 1f);
            if (randChance < 0.5) chance = true;
            else chance = false;

            conditions[rand + i.ToString()] = chance;
        }

        conditionalDialogue.Clear();
        List<string> di1 = new List<string>();
        List<string> di2 = new List<string>();
        List<string> di3 = new List<string>();
        List<string> di4 = new List<string>();

        di1.Add("Hello! Welcome to the start of the Tech Demo where I display that dialogue works! WOWZA!");
        di1.Add("If you are reading this, you ended up in a timeline where options 1 and 2 in my production system are true.");
        di1.Add("Press E and then W to see what you get next time or press S to continue to the map.");
        di2.Add("Hello! Welcome to the start of the Tech Demo where I display that dialogue works! WOWZA!");
        di2.Add("If you are reading this, you ended up in a timeline where option 2 in my production system is true.");
        di2.Add("Press E and then W to see what you get next time or press S to continue to the map.");
        di3.Add("Hello! Welcome to the start of the Tech Demo where I display that dialogue works! WOWZA!");
        di3.Add("If you are reading this, you ended up in a timeline where option 1 in my production system is true.");
        di3.Add("Press E and then W to see what you get next time or press S to continue to the map.");
        di4.Add("Hello! Welcome to the start of the Tech Demo where I display that dialogue works! WOWZA!");
        di4.Add("If you are reading this, you ended up in a timeline where no options in my production system are true.");
        di4.Add("Press E and then W to see what you get next time or press S to continue to the map.");

        conditionalDialogue.Add("12", di1);
        conditionalDialogue.Add("2", di2);
        conditionalDialogue.Add("1", di3);
        conditionalDialogue.Add("0", di4);
    }

    public void ResetSystem()
    {
        initDemoSystem();
        hasGiven = false;
        print("system has reset");
    }
}
