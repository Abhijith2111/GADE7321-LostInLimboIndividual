using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerialisedDialogue
{
    public SerialisedDialogueItem[] dialogueItems;

    public Dialogue Deserialised
    {
        get
        {
            List<DialogueItem> items = new List<DialogueItem>();
            Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
            foreach (SerialisedDialogueItem serialisedItem in dialogueItems)
            {
                if (!sprites.ContainsKey(serialisedItem.icon))
                {
                    Sprite sprite = Resources.Load<Sprite>(serialisedItem.icon);
                    sprites.Add(serialisedItem.icon, sprite);
                }
                DialogueItem item = serialisedItem.Deserialised;
                item.Icon = sprites[serialisedItem.icon];
                items.Add(item);
            }
            return new Dialogue() { DialogueItems = items };
        }
    }
}

[Serializable]
public class Dialogue
{
    public List<DialogueItem> DialogueItems;

    public SerialisedDialogue Serialised
    {
        get
        {
            List<SerialisedDialogueItem> serialisedItems = new List<SerialisedDialogueItem>();
            foreach (DialogueItem item in DialogueItems)
            {
                serialisedItems.Add(item.Serialised);
            }
            return new SerialisedDialogue() { dialogueItems = serialisedItems.ToArray() };
        }
    }

    public void UnloadAllSprites()
    {
        HashSet<Sprite> sprites = new HashSet<Sprite>();
		foreach (DialogueItem item in DialogueItems)
		{
            sprites.Add(item.Icon);
		}
        foreach (Sprite icon in sprites)
        {
            Resources.UnloadAsset(icon);
        }
	}
}
