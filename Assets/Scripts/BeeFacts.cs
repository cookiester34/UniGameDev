using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeeFacts {
    private static string[] facts = new[] {
        "Did you know that all worker bees are female?",
        "Did you know that in summer a colony could have up to 50000 bees?",
        "Did you know that a bee will visit over 1500 flowers for only one load of pollen?",
        "Did you know that bees only live for 3 weeks?",
        "Each bee will fly about 800km in her 3-week lifetime.",
        "Bees are covered by tiny hairs, which help spread the pollen from one flower to another to help new flowers grow.",
        "Bees are 100 times more sensitive to the smell of flowers than humans.",
        "Did you know that bees love dancing? The foraging bees do a “waggle dance” to tell bees in their hive know where the flowers are.",
        "About a third of the world’s food supply depends on pollinating insects like honeybees. ",
        "According to Albert Einstein, if honeybees were to disappear from earth, humanity would die within 4 years. ",
        "A hexagonal shape of a honeycomb cell is the most optimal shape for the beehive.",
        "Honeybees have a soul of a dancer; they communicate through dance.",
        "Only female bees can sting.",
        "Male honeybees are kicked out of the hive when they cannot mate.",
        "Honeybee attack smells like bananas. ",
        "Bees have a tendency to collect toxic chemicals. ",
        "Honeybees can recognize human faces.",
        "The queen bee can select the sex of their larvae. ",
        "Honeybees love coffee!",
        "For a very long-time scientists were not able to figure out how bees are able to fly. ",
        "A single bee produces only 1/12 of a teaspoon of honey during its lifetime.",
        "Honeybees can detect explosives. ",
        "A honeybee a colony is technically immortal. ",
        "Approximately 80% of insect crop pollination is done by the honeybees. ",
        "Pollen affects honeybee’s development and productivity of the whole colony. ",
        "What do you call a bee born in May?\nA maybe!",
        "What did the bee cross the road?\nJust bee-cause!",
        "What kind of bees drop things?\nFumble bees!",
        "What kind of bees can’t be understood?\nMumble bees!",
        "Where do bees like to go on holiday?\nTo the bee-ch!",
        "What do bees wear to the beach?\nBee-kinis!",
        "What does a bee sit on?\nIts bee-hind!",
        "Where do bees cook their dinner?\nOn the bar-bee-que!",
        "Where do bees wait for buses?\nAt buzz stops!",
        "What kind of bees live in graveyards?\nZom-bees!"

    };

    private static int lastIndex = 0;

    static BeeFacts() {
        lastIndex = Random.Range(0, facts.Length - 1);
    }

    public static string GetFact() {
        int index = Random.Range(0, facts.Length - 1);
        if (index == lastIndex) {
            index++;
            index = Mathf.Clamp(index, 0, facts.Length - 1);
        }

        lastIndex = index;
        return facts[index];
    }
}
