using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using Microsoft.Win32;

namespace BlakApp.Views.Pages
{
    public partial class SequenceAnalyzerView : UserControl
    {
        public SequenceAnalyzerView()
        {
            InitializeComponent();
/*
            //First of all you need to have a reference for your TextEditor for it to be used inside AvaloniaEdit.TextMate project.
            var textEditor = this.FindControl<TextEditor>("Editor");
        
            textEditor.AppendText("<TextBlock Text=\"test\" Name=\"example\" Foreground=\"Red\" FontSize=\"24\"/>");
            textEditor.Document.Insert(0, "3fsfds");
            textEditor.Document.Insert(0, "3fsfds");
            textEditor.Document.Insert(0, "3fsfds");

            var richTextModel = new RichTextModel();

            richTextModel.SetForeground(0, 10, new SimpleHighlightingBrush(Colors.Red));
            richTextModel.SetForeground(15, 20, new SimpleHighlightingBrush(Colors.Red));
            
            var richText = new RichText("eddrftyguhinjimfdifbsdfssdfds",richTextModel);

            var rulesSet = new HighlightingRuleSet();
            rulesSet.Rules.Add(new HighlightingRule() 
            { 
                Color = new HighlightingColor() { Foreground = new SimpleHighlightingBrush(Colors.Red)},
            });

            HighlightingEngine engine = new HighlightingEngine(rulesSet);

            textEditor.SyntaxHighlighting = rulesSet;*/
        }
    }
}
