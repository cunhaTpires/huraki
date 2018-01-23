using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using LuisBot.Dialogs;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        public LuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        #region Example DeprecatedIntents
        //private readonly Dictionary<string, Note> noteByTitle  = new Dictionary<string, Note>();
        //private Note noteToCreate;
        //private string currentTitle;

        //public const string Entity_Note_Title = "Note.Tittle";
        //public const string DefaultNoteTitle = "default";

        //[Serializable]
        //public sealed class Note : IEquatable<Note>
        //{
        //    public string Title { get; set; }
        //    public string Text { get; set; }

        //    public override string ToString()
        //    {
        //        return "[{this.Title} : {this.Text}]";
        //    }
        //    public bool Equals(Note other)
        //    {
        //        return other != null && this.Text == other.Text && this.Title == other.Title;
        //    }
        //    public override bool Equals(object other)
        //    {
        //        return Equals(other as Note);
        //    }
        //    public override int GetHashCode()
        //    {
        //        return this.Title.GetHashCode();
        //    }
        //}

        //[LuisIntent("Note.Create")]
        //public Task NoteCreateIntent(IDialogContext context, LuisResult result)
        //{
        //    EntityRecommendation title;
        //    if (!result.TryFindEntity(Entity_Note_Title, out title))
        //    {
        //        // Prompt the user for a note title
        //        PromptDialog.Text(context, After_TitlePrompt, "What is the title of the note you want to create?");
        //    }
        //    else
        //    {
        //        var note = new Note()
        //        {
        //            Title = title.Entity
        //        };
        //        noteToCreate = this.noteByTitle[note.Title] = note;

        //        // Prompt the user for what they want to say in the note
        //        PromptDialog.Text(context, After_TextPrompt, "What do you want to say in your note?");
        //    }
        //    return Task.CompletedTask;
        //}

        //private async Task After_TitlePrompt(IDialogContext context, IAwaitable<string> result)
        //{
        //    EntityRecommendation title;
        //    // Set the title (used for creation, deletion, and reading)
        //    currentTitle = await result;
        //    if (currentTitle != null)
        //    {
        //        title = new EntityRecommendation(type: Entity_Note_Title)
        //        {
        //            Entity = currentTitle
        //        };
        //    }
        //    else
        //    {
        //        // Use the default note title
        //        title = new EntityRecommendation(type: Entity_Note_Title)
        //        {
        //            Entity = DefaultNoteTitle
        //        };
        //    }

        //    // Create a new note object
        //    var note = new Note()
        //    {
        //        Title = title.Entity
        //    };
        //    // Add the new note to the list of notes and also save it in order to add text to it later
        //    noteToCreate = this.noteByTitle[note.Title] = note;

        //    // Prompt the user for what they want to say in the note
        //    PromptDialog.Text(context, After_TextPrompt, "What do you want to say in your note?");
        //}

        //private async Task After_TextPrompt(IDialogContext context, IAwaitable<string> result)
        //{
        //    // Set the text of the note
        //    noteToCreate.Text = await result;

        //    await context.PostAsync(string.Format("Created note **{0}** that says \"{1}\".", noteToCreate.Title, noteToCreate.Text));
        //    context.Wait(MessageReceived);
        //}

        //[LuisIntent("Note.ReadAloud")]
        //public async Task NoteReadAloudIntent(IDialogContext context, LuisResult result)
        //{
        //    Note note;
        //    if (TryFindNote(result, out note))
        //    {
        //        await context.PostAsync(string.Format("**{0}**: {1}.", note.Title, note.Text));
        //    }
        //    else
        //    {
        //        // Print out all the notes if no specific note name was detected
        //        string NoteList = "Here's the list of all notes: \n\n";
        //        foreach (KeyValuePair<string, Note> entry in noteByTitle)
        //        {
        //            Note noteInList = entry.Value;
        //            NoteList += "**{noteInList.Title}**: {noteInList.Text}.\n\n";
        //        }
        //        await context.PostAsync(NoteList);
        //    }
        //    context.Wait(MessageReceived);
        //}

        //public bool TryFindNote(string noteTitle, out Note note)
        //{
        //    // TryGetValue returns false if no match is found.
        //    bool foundNote = this.noteByTitle.TryGetValue(noteTitle, out note);
        //    return foundNote;
        //}

        //public bool TryFindNote(LuisResult result, out Note note)
        //{
        //    note = null;

        //    string titleToFind;

        //    EntityRecommendation title;
        //    if (result.TryFindEntity(Entity_Note_Title, out title))
        //    {
        //        titleToFind = title.Entity;
        //    }
        //    else
        //    {
        //        titleToFind = DefaultNoteTitle;
        //    }

        //    // TryGetValue returns false if no match is found.
        //    return this.noteByTitle.TryGetValue(titleToFind, out note);
        //}

        //[LuisIntent("Cancel")]
        //public async Task CancelIntent(IDialogContext context, LuisResult result)
        //{
        //    await this.ShowLuisResult(context, result);
        //}

        //[LuisIntent("Help")]
        //public async Task HelpIntent(IDialogContext context, LuisResult result)
        //{
        //    await this.ShowLuisResult(context, result);
        //}
        #endregion


        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I do not know'{result.Query}'";

            QnADialog qna = new QnADialog();
            if(!qna.TryQuery(result.Query, out message))
                message = $"Sorry, I do not know'{result.Query}'";
            await context.PostAsync(message);
            context.Done(1);    // Go back to Root
            //await this.ShowLuisResult(context, result);
        }

        //Try find answers QnAService


        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Gretting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}