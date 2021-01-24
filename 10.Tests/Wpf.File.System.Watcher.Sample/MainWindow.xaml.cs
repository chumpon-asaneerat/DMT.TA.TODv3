#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

// NJson
using DMT;

// AvalonEdit
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit.Indentation.Json;
using System.Windows.Media;
using System.Xml;

#endregion

namespace Wpf.FileSystemWatchers.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private FileSystemWatcher watcher = null;
        private SampleJson data = new SampleJson();

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            LoadSyntax();
            LoadEditor();

            cmdStart.IsEnabled = true;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;

            txtPath.Text = PathName;
            txtFileName.Text = "sample.json";
        }

        private void Window_Unloaded(Object sender, RoutedEventArgs e)
        {
            Shutdown();
        }

        #endregion

        #region Button Handlers

        private void cmdOpenNotepad_Click(object sender, RoutedEventArgs e)
        {
            string fileName = FileName;
            if (string.IsNullOrWhiteSpace(fileName)) return;
            if (File.Exists(fileName))
            {
                Process.Start("notepad.exe", fileName);
            }
        }

        private void cmdStart_Click(Object sender, RoutedEventArgs e)
        {
            cmdStart.IsEnabled = false;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;

            Start();
        }

        private void cmdShutdown_Click(Object sender, RoutedEventArgs e)
        {
            Shutdown();

            cmdStart.IsEnabled = true;
            cmdShutdown.IsEnabled = !cmdStart.IsEnabled;
        }

        private void cmdLoad_Click(Object sender, RoutedEventArgs e)
        {
            LoadFile();
        }

        private void cmdSave_Click(Object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        #endregion

        #region File System Watcher Handlers

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() => 
            {
                if (!string.IsNullOrWhiteSpace(e.FullPath) && !string.IsNullOrWhiteSpace(this.FileName) &&
                    e.FullPath.Trim().ToLower() == this.FileName.Trim().ToLower())
                {
                    LoadFile();
                    /*
                    // Gets Last Write Time.
                    DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
                    TimeSpan ts = lastWriteTime - _lastRead;
                    if (ts.TotalMilliseconds > 0)
                    {
                        Console.WriteLine("Detected File '{0}' Changed.", e.Name);
                        // Reload config.
                        this.LoadConfig();
                        // Set last read.
                        _lastRead = lastWriteTime;
                    }
                    */
                }
            });

        }

        #endregion

        #region Private Methods

        private void LoadSyntax()
        {
            //var assembly = Assembly.GetAssembly(typeof(ICSharpCode.AvalonEdit.TextEditor));
            var assembly = Assembly.GetAssembly(this.GetType());
            using (var stream = assembly.GetManifestResourceStream("Wpf.FileSystemWatchers.Sample.Json.xshd"))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    editor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    //SearchPanel.Install(avalonEdit);
                }
            }
        }

        private void LoadEditor()
        {
            editor.Options.ConvertTabsToSpaces = true;
            editor.Options.EnableRectangularSelection = true;
            editor.Options.IndentationSize = 4;
            editor.ShowLineNumbers = true;

            editor.TextArea.IndentationStrategy = new JsonIndentationStrategy(editor.Options);
            //editor.TextArea.IndentationStrategy = new DefaultIndentationStrategy();

            LoadSyntax();
            //editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".xml");

            var foldingManager = FoldingManager.Install(editor.TextArea);
            var foldingStrategy = new BraceFoldingStrategy();
            SetValue(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display);
            editor.TextArea.IndentationStrategy = new DefaultIndentationStrategy();
            editor.TextChanged += (s, e) => foldingStrategy.UpdateFoldings(foldingManager, ((TextEditor)s).Document);

            editor.Text = "";
        }

        private string PathName
        {
            get { return NJson.LocalConfigFolder; }
        }

        private string FileName
        {
            get
            {
                string fileName = txtFileName.Text.Trim();
                if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;
                return Path.Combine(PathName, fileName);
            }
        }

        private void LoadFile()
        {
            editor.Text = string.Empty;

            string fileName = FileName;
            if (string.IsNullOrEmpty(fileName)) return;
            if (!File.Exists(fileName)) return;

            data = NJson.LoadFromFile<SampleJson>(fileName);
            if (null == data) return;

            string json = data.ToJson();
            editor.Text = json;
        }

        private void SaveFile()
        {
            string fileName = FileName;
            if (string.IsNullOrEmpty(fileName)) return;

            string json = editor.Text;
            if (string.IsNullOrEmpty(json)) return;
            data = json.FromJson<SampleJson>();
            if (null == data) return;
            data.SaveToFile(fileName);
        }


        private void Start()
        {
            if (null != watcher) return;
            if (string.IsNullOrEmpty(PathName)) return;

            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Path = PathName;
            watcher.Filter = "*.json";
;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += Watcher_Changed;
        }

        private void Shutdown()
        {
            if (null != watcher)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= Watcher_Changed;
                watcher.Dispose();
            }
            watcher = null;
        }

        #endregion
    }

    public class SampleJson
    {
        public SampleJson()
        {
            this.IP = new IP();
            this.Items = new List<EditItem>();
            this.Items.Add(new EditItem() { Name = "Item 1", UpdateDateTime = DateTime.Now });
            this.Items.Add(new EditItem() { Name = "Item 2", UpdateDateTime = new DateTime?() }); ;
            this.Items.Add(new EditItem() { Name = "Item 3", UpdateDateTime = DateTime.Now });
        }

        public IP IP { get; set; }
        public List<EditItem> Items { get; set; }
    }

    public class IP
    {
        public string HostName { get; set; }
    }

    public class EditItem
    {
        public string Name { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}

namespace ICSharpCode.AvalonEdit.Folding
{
    /// <summary>
    /// Allows producing foldings from a document based on braces.
    /// </summary>
    public class BraceFoldingStrategy
    {
        // How many spaces == one tab
        private const int SpacesInTab = 4;

        /// <summary>
        /// Gets/Sets the opening brace. The default value is '{'.
        /// </summary>
        public char OpeningBrace { get; set; }

        /// <summary>
        /// Gets/Sets the closing brace. The default value is '}'.
        /// </summary>
        public char ClosingBrace { get; set; }

        /// <summary>
        /// Creates a new BraceFoldingStrategy.
        /// </summary>
        public BraceFoldingStrategy()
        {
            this.OpeningBrace = '{';
            this.ClosingBrace = '}';
        }

        public void UpdateFoldings(FoldingManager manager, TextDocument document)
        {
            int firstErrorOffset;
            IEnumerable<NewFolding> newFoldings = CreateNewFoldings(document, out firstErrorOffset);
            manager.UpdateFoldings(newFoldings, firstErrorOffset);
        }

        /// <summary>
        /// Create <see cref="NewFolding"/>s for the specified document.
        /// </summary>
        public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
        {
            firstErrorOffset = -1;
            return CreateNewFoldings(document);
            //return CreateNewFoldingsByLine(document);
        }

        /// <summary>
        /// Create <see cref="NewFolding"/>s for the specified document.
        /// </summary>
        public IEnumerable<NewFolding> CreateNewFoldingsByLine(ITextSource document)
        {
            List<NewFolding> newFoldings = new List<NewFolding>();

            if (document == null || (document as TextDocument).LineCount <= 1)
            {
                return newFoldings;
            }

            //Can keep track of offset ourself and from testing it seems to be accurate
            int offsetTracker = 0;

            // Keep track of start points since things nest
            Stack<int> startOffsets = new Stack<int>();

            StringBuilder lineBuffer = new StringBuilder();

            foreach (DocumentLine line in (document as TextDocument).Lines)
            {
                if (offsetTracker >= document.TextLength)
                {
                    break;
                }

                lineBuffer.Clear();


                // First task is to get the line and figure out the spacing in front of it
                int spaceCounter = 0;
                bool foundText = false;
                bool foundColon = false;
                //for (int i = 0; i < line.Length; i++)
                int i = 0;
                //TODO buffer the characters so you can have the line contents on the stack too for the folding name (display text)
                while (i < line.Length && !(foundText && foundColon))
                {
                    char c = document.GetCharAt(offsetTracker + i);

                    switch (c)
                    {
                        case ' ': // spaces count as one
                            if (!foundText)
                            {
                                spaceCounter++;
                            }
                            break;
                        case '\t': // Tabs count as N
                            if (!foundText)
                            {
                                spaceCounter += SpacesInTab;
                            }
                            break;
                        case ':': // Tabs count as N
                            foundColon = true;
                            break;
                        default: // anything else means we encountered not spaces or tabs, so keep making the line but stop counting
                            foundText = true;
                            break;
                    }
                    i++;
                }

                // before we continue, we need to make sure its a correct multiple
                int remainder = spaceCounter % SpacesInTab;
                if (remainder > 0)
                {
                    // Some tabbing isn't correct. ignore this line for folding purposes.
                    // This may break all foldings below that, but it's a complex problem to address.
                    continue;
                }

                // Now we need to figure out if this line is a new folding by checking its tabing
                // relative to the current stack count. Convert into virtual tabs and compare to stack level
                int numTabs = spaceCounter / SpacesInTab; // we know this will be an int because of the above check
                if (numTabs >= startOffsets.Count && foundText && foundColon)
                {
                    // we are starting a new folding
                    startOffsets.Push(offsetTracker);

                }
                else // numtabs < offsets
                {
                    // we know that this is the end of a folding. It could be the end of multiple foldings. So pop until it matches.
                    while (numTabs < startOffsets.Count)
                    {
                        int foldingStart = startOffsets.Pop();
                        NewFolding tempFolding = new NewFolding();
                        //tempFolding.Name = < could add logic here, possibly by tracking key words when starting the folding, to control what is shown when it's folded >
                        tempFolding.StartOffset = foldingStart;
                        tempFolding.EndOffset = offsetTracker - 2;
                        newFoldings.Add(tempFolding);
                    }
                }


                // Increment tracker. Much faster than getting it from the line
                offsetTracker += line.TotalLength;
            }

            // Complete last foldings
            while (startOffsets.Count > 0)
            {
                int foldingStart = startOffsets.Pop();
                NewFolding tempFolding = new NewFolding();
                //tempFolding.Name = < could add logic here, possibly by tracking key words when starting the folding, to control what is shown when it's folded >
                tempFolding.StartOffset = foldingStart;
                tempFolding.EndOffset = offsetTracker;
                newFoldings.Add(tempFolding);
            }

            newFoldings.Sort((a, b) => (a.StartOffset.CompareTo(b.StartOffset)));
            return newFoldings;
        }

        /// <summary>
        /// Create <see cref="NewFolding"/>s for the specified document.
        /// </summary>
        public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
        {
            List<NewFolding> newFoldings = new List<NewFolding>();

            Stack<int> startOffsets = new Stack<int>();
            int lastNewLineOffset = 0;
            char openingBrace = this.OpeningBrace;
            char closingBrace = this.ClosingBrace;
            for (int i = 0; i < document.TextLength; i++)
            {
                char c = document.GetCharAt(i);
                if (c == openingBrace)
                {
                    startOffsets.Push(i);
                }
                else if (c == closingBrace && startOffsets.Count > 0)
                {
                    int startOffset = startOffsets.Pop();
                    // don't fold if opening and closing brace are on the same line
                    if (startOffset < lastNewLineOffset)
                    {
                        newFoldings.Add(new NewFolding(startOffset, i + 1));
                    }
                }
                else if (c == '\n' || c == '\r')
                {
                    lastNewLineOffset = i + 1;
                }
            }
            newFoldings.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));
            return newFoldings;
        }
    }
}

namespace ICSharpCode.AvalonEdit.Indentation.Json
{
    using System.Globalization;

    #region IDocumentAccessor

    /// <summary>
    /// Interface used for the indentation class to access the document.
    /// </summary>
    public interface IDocumentAccessor
    {
        /// <summary>Gets if the current line is read only (because it is not in the
        /// selected text region)</summary>
        bool IsReadOnly { get; }
        /// <summary>Gets the number of the current line.</summary>
        int LineNumber { get; }
        /// <summary>Gets/Sets the text of the current line.</summary>
        string Text { get; set; }
        /// <summary>Advances to the next line.</summary>
        bool MoveNext();
    }

    #endregion

    #region TextDocumentAccessor

    /// <summary>
    /// Adapter IDocumentAccessor -> TextDocument
    /// </summary>
    public sealed class TextDocumentAccessor : IDocumentAccessor
    {
        readonly TextDocument doc;
        readonly int minLine;
        readonly int maxLine;

        /// <summary>
        /// Creates a new TextDocumentAccessor.
        /// </summary>
        public TextDocumentAccessor(TextDocument document)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            doc = document;
            this.minLine = 1;
            this.maxLine = doc.LineCount;
        }

        /// <summary>
        /// Creates a new TextDocumentAccessor that indents only a part of the document.
        /// </summary>
        public TextDocumentAccessor(TextDocument document, int minLine, int maxLine)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            doc = document;
            this.minLine = minLine;
            this.maxLine = maxLine;
        }

        int num;
        string text;
        DocumentLine line;

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get
            {
                return num < minLine;
            }
        }

        /// <inheritdoc/>
        public int LineNumber
        {
            get
            {
                return num;
            }
        }

        bool lineDirty;

        /// <inheritdoc/>
        public string Text
        {
            get { return text; }
            set
            {
                if (num < minLine) return;
                text = value;
                lineDirty = true;
            }
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (lineDirty)
            {
                doc.Replace(line, text);
                lineDirty = false;
            }
            ++num;
            if (num > maxLine) return false;
            line = doc.GetLineByNumber(num);
            text = doc.GetText(line);
            return true;
        }
    }

    #endregion

    sealed class IndentationSettings
    {
        public string IndentString = "\t";
        /// <summary>Leave empty lines empty.</summary>
        public bool LeaveEmptyLines = true;
    }

    sealed class IndentationReformatter
    {
        /// <summary>
        /// An indentation block. Tracks the state of the indentation.
        /// </summary>
        struct Block
        {
            /// <summary>
            /// The indentation outside of the block.
            /// </summary>
            public string OuterIndent;

            /// <summary>
            /// The indentation inside the block.
            /// </summary>
            public string InnerIndent;

            /// <summary>
            /// The last word that was seen inside this block.
            /// Because parenthesis open a sub-block and thus don't change their parent's LastWord,
            /// this property can be used to identify the type of block statement (if, while, switch)
            /// at the position of the '{'.
            /// </summary>
            public string LastWord;

            /// <summary>
            /// The type of bracket that opened this block (, [ or {
            /// </summary>
            public char Bracket;

            /// <summary>
            /// Gets whether there's currently a line continuation going on inside this block.
            /// </summary>
            public bool Continuation;

            /// <summary>
            /// Gets whether there's currently a 'one-line-block' going on. 'one-line-blocks' occur
            /// with if statements that don't use '{}'. They are not represented by a Block instance on
            /// the stack, but are instead handled similar to line continuations.
            /// This property is an integer because there might be multiple nested one-line-blocks.
            /// As soon as there is a finished statement, OneLineBlock is reset to 0.
            /// </summary>
            public int OneLineBlock;

            /// <summary>
            /// The previous value of one-line-block before it was reset.
            /// Used to restore the indentation of 'else' to the correct level.
            /// </summary>
            public int PreviousOneLineBlock;

            public void ResetOneLineBlock()
            {
                PreviousOneLineBlock = OneLineBlock;
                OneLineBlock = 0;
            }

            /// <summary>
            /// Gets the line number where this block started.
            /// </summary>
            public int StartLine;

            public void Indent(IndentationSettings set)
            {
                Indent(set.IndentString);
            }

            public void Indent(string indentationString)
            {
                OuterIndent = InnerIndent;
                InnerIndent += indentationString;
                Continuation = false;
                ResetOneLineBlock();
                LastWord = "";
            }

            public override string ToString()
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "[Block StartLine={0}, LastWord='{1}', Continuation={2}, OneLineBlock={3}, PreviousOneLineBlock={4}]",
                    this.StartLine, this.LastWord, this.Continuation, this.OneLineBlock, this.PreviousOneLineBlock);
            }
        }

        StringBuilder wordBuilder;
        Stack<Block> blocks; // blocks contains all blocks outside of the current
        Block block;  // block is the current block

        bool inString;
        bool inChar;
        bool verbatim;
        bool escape;

        bool lineComment;
        bool blockComment;

        char lastRealChar; // last non-comment char

        public void Reformat(IDocumentAccessor doc, IndentationSettings set)
        {
            Init();

            while (doc.MoveNext())
            {
                Step(doc, set);
            }
        }

        public void Init()
        {
            wordBuilder = new StringBuilder();
            blocks = new Stack<Block>();
            block = new Block();
            block.InnerIndent = "";
            block.OuterIndent = "";
            block.Bracket = '{';
            block.Continuation = false;
            block.LastWord = "";
            block.OneLineBlock = 0;
            block.PreviousOneLineBlock = 0;
            block.StartLine = 0;

            inString = false;
            inChar = false;
            verbatim = false;
            escape = false;

            lineComment = false;
            blockComment = false;

            lastRealChar = ' '; // last non-comment char
        }

        public void Step(IDocumentAccessor doc, IndentationSettings set)
        {
            string line = doc.Text;
            if (set.LeaveEmptyLines && line.Length == 0) return; // leave empty lines empty
            line = line.TrimStart();

            StringBuilder indent = new StringBuilder();
            if (line.Length == 0)
            {
                // Special treatment for empty lines:
                if (blockComment || (inString && verbatim))
                    return;
                indent.Append(block.InnerIndent);
                indent.Append(Repeat(set.IndentString, block.OneLineBlock));
                if (block.Continuation)
                    indent.Append(set.IndentString);
                if (doc.Text != indent.ToString())
                    doc.Text = indent.ToString();
                return;
            }

            if (TrimEnd(doc))
                line = doc.Text.TrimStart();

            Block oldBlock = block;
            bool startInComment = blockComment;
            bool startInString = (inString && verbatim);

            #region Parse char by char
            lineComment = false;
            inChar = false;
            escape = false;
            if (!verbatim) inString = false;

            lastRealChar = '\n';

            char lastchar = ' ';
            char c = ' ';
            char nextchar = line[0];
            for (int i = 0; i < line.Length; i++)
            {
                if (lineComment) break; // cancel parsing current line

                lastchar = c;
                c = nextchar;
                if (i + 1 < line.Length)
                    nextchar = line[i + 1];
                else
                    nextchar = '\n';

                if (escape)
                {
                    escape = false;
                    continue;
                }

                #region Check for comment/string chars
                switch (c)
                {
                    case '/':
                        if (blockComment && lastchar == '*')
                            blockComment = false;
                        if (!inString && !inChar)
                        {
                            if (!blockComment && nextchar == '/')
                                lineComment = true;
                            if (!lineComment && nextchar == '*')
                                blockComment = true;
                        }
                        break;
                    case '#':
                        if (!(inChar || blockComment || inString))
                            lineComment = true;
                        break;
                    case '"':
                        if (!(inChar || lineComment || blockComment))
                        {
                            inString = !inString;
                            if (!inString && verbatim)
                            {
                                if (nextchar == '"')
                                {
                                    escape = true; // skip escaped quote
                                    inString = true;
                                }
                                else
                                {
                                    verbatim = false;
                                }
                            }
                            else if (inString && lastchar == '@')
                            {
                                verbatim = true;
                            }
                        }
                        break;
                    case '\'':
                        if (!(inString || lineComment || blockComment))
                        {
                            inChar = !inChar;
                        }
                        break;
                    case '\\':
                        if ((inString && !verbatim) || inChar)
                            escape = true; // skip next character
                        break;
                }
                #endregion

                if (lineComment || blockComment || inString || inChar)
                {
                    if (wordBuilder.Length > 0)
                        block.LastWord = wordBuilder.ToString();
                    wordBuilder.Length = 0;
                    continue;
                }

                if (!Char.IsWhiteSpace(c) && c != '[' && c != '/')
                {
                    if (block.Bracket == '{')
                        block.Continuation = true;
                }

                if (Char.IsLetterOrDigit(c))
                {
                    wordBuilder.Append(c);
                }
                else
                {
                    if (wordBuilder.Length > 0)
                        block.LastWord = wordBuilder.ToString();
                    wordBuilder.Length = 0;
                }

                #region Push/Pop the blocks
                switch (c)
                {
                    case '{':
                        block.ResetOneLineBlock();
                        blocks.Push(block);
                        block.StartLine = doc.LineNumber;
                        if (block.LastWord == "switch")
                        {
                            block.Indent(set.IndentString + set.IndentString);
                            /* oldBlock refers to the previous line, not the previous block
                             * The block we want is not available anymore because it was never pushed.
                             * } else if (oldBlock.OneLineBlock) {
                            // Inside a one-line-block is another statement
                            // with a full block: indent the inner full block
                            // by one additional level
                            block.Indent(set, set.IndentString + set.IndentString);
                            block.OuterIndent += set.IndentString;
                            // Indent current line if it starts with the '{' character
                            if (i == 0) {
                                oldBlock.InnerIndent += set.IndentString;
                            }*/
                        }
                        else
                        {
                            block.Indent(set);
                        }
                        block.Bracket = '{';
                        break;
                    case '}':
                        while (block.Bracket != '{')
                        {
                            if (blocks.Count == 0) break;
                            block = blocks.Pop();
                        }
                        if (blocks.Count == 0) break;
                        block = blocks.Pop();
                        block.Continuation = false;
                        block.ResetOneLineBlock();
                        break;
                    case '(':
                    case '[':
                        blocks.Push(block);
                        if (block.StartLine == doc.LineNumber)
                            block.InnerIndent = block.OuterIndent;
                        else
                            block.StartLine = doc.LineNumber;
                        block.Indent(Repeat(set.IndentString, oldBlock.OneLineBlock) +
                                     (oldBlock.Continuation ? set.IndentString : "") +
                                     (i == line.Length - 1 ? set.IndentString : new String(' ', i + 1)));
                        block.Bracket = c;
                        break;
                    case ')':
                        if (blocks.Count == 0) break;
                        if (block.Bracket == '(')
                        {
                            block = blocks.Pop();
                            if (IsSingleStatementKeyword(block.LastWord))
                                block.Continuation = false;
                        }
                        break;
                    case ']':
                        if (blocks.Count == 0) break;
                        if (block.Bracket == '[')
                            block = blocks.Pop();
                        break;
                    case ';':
                    case ',':
                        block.Continuation = false;
                        block.ResetOneLineBlock();
                        break;
                    case ':':
                        if (block.LastWord == "case"
                            || line.StartsWith("case ", StringComparison.Ordinal)
                            || line.StartsWith(block.LastWord + ":", StringComparison.Ordinal))
                        {
                            block.Continuation = false;
                            block.ResetOneLineBlock();
                        }
                        break;
                }

                if (!Char.IsWhiteSpace(c))
                {
                    // register this char as last char
                    lastRealChar = c;
                }
                #endregion
            }
            #endregion

            if (wordBuilder.Length > 0)
                block.LastWord = wordBuilder.ToString();
            wordBuilder.Length = 0;

            if (startInString) return;
            if (startInComment && line[0] != '*') return;
            if (doc.Text.StartsWith("//\t", StringComparison.Ordinal) || doc.Text == "//")
                return;

            if (line[0] == '}')
            {
                indent.Append(oldBlock.OuterIndent);
                oldBlock.ResetOneLineBlock();
                oldBlock.Continuation = false;
            }
            else
            {
                indent.Append(oldBlock.InnerIndent);
            }

            if (indent.Length > 0 && oldBlock.Bracket == '(' && line[0] == ')')
            {
                indent.Remove(indent.Length - 1, 1);
            }
            else if (indent.Length > 0 && oldBlock.Bracket == '[' && line[0] == ']')
            {
                indent.Remove(indent.Length - 1, 1);
            }

            if (line[0] == ':')
            {
                oldBlock.Continuation = true;
            }
            else if (lastRealChar == ':' && indent.Length >= set.IndentString.Length)
            {
                if (block.LastWord == "case" || line.StartsWith("case ", StringComparison.Ordinal) || line.StartsWith(block.LastWord + ":", StringComparison.Ordinal))
                    indent.Remove(indent.Length - set.IndentString.Length, set.IndentString.Length);
            }
            else if (lastRealChar == ')')
            {
                if (IsSingleStatementKeyword(block.LastWord))
                {
                    block.OneLineBlock++;
                }
            }
            else if (lastRealChar == 'e' && block.LastWord == "else")
            {
                block.OneLineBlock = Math.Max(1, block.PreviousOneLineBlock);
                block.Continuation = false;
                oldBlock.OneLineBlock = block.OneLineBlock - 1;
            }

            if (doc.IsReadOnly)
            {
                // We can't change the current line, but we should accept the existing
                // indentation if possible (=if the current statement is not a multiline
                // statement).
                if (!oldBlock.Continuation && oldBlock.OneLineBlock == 0 &&
                    oldBlock.StartLine == block.StartLine &&
                    block.StartLine < doc.LineNumber && lastRealChar != ':')
                {
                    // use indent StringBuilder to get the indentation of the current line
                    indent.Length = 0;
                    line = doc.Text; // get untrimmed line
                    for (int i = 0; i < line.Length; ++i)
                    {
                        if (!Char.IsWhiteSpace(line[i]))
                            break;
                        indent.Append(line[i]);
                    }
                    // /* */ multiline comments have an extra space - do not count it
                    // for the block's indentation.
                    if (startInComment && indent.Length > 0 && indent[indent.Length - 1] == ' ')
                    {
                        indent.Length -= 1;
                    }
                    block.InnerIndent = indent.ToString();
                }
                return;
            }

            if (line[0] != '{')
            {
                if (line[0] != ')' && oldBlock.Continuation && oldBlock.Bracket == '{')
                    indent.Append(set.IndentString);
                indent.Append(Repeat(set.IndentString, oldBlock.OneLineBlock));
            }

            // this is only for blockcomment lines starting with *,
            // all others keep their old indentation
            if (startInComment)
                indent.Append(' ');

            if (indent.Length != (doc.Text.Length - line.Length) ||
                !doc.Text.StartsWith(indent.ToString(), StringComparison.Ordinal) ||
                Char.IsWhiteSpace(doc.Text[indent.Length]))
            {
                doc.Text = indent.ToString() + line;
            }
        }

        static string Repeat(string text, int count)
        {
            if (count == 0)
                return string.Empty;
            if (count == 1)
                return text;
            StringBuilder b = new StringBuilder(text.Length * count);
            for (int i = 0; i < count; i++)
                b.Append(text);
            return b.ToString();
        }

        static bool IsSingleStatementKeyword(string keyword)
        {
            switch (keyword)
            {
                case "if":
                case "for":
                case "while":
                case "do":
                case "foreach":
                case "using":
                case "lock":
                    return true;
                default:
                    return false;
            }
        }

        static bool TrimEnd(IDocumentAccessor doc)
        {
            string line = doc.Text;
            if (!Char.IsWhiteSpace(line[line.Length - 1])) return false;

            // one space after an empty comment is allowed
            if (line.EndsWith("// ", StringComparison.Ordinal) || line.EndsWith("* ", StringComparison.Ordinal))
                return false;

            doc.Text = line.TrimEnd();
            return true;
        }
    }

    /// <summary>
    /// Smart indentation for Json.
    /// </summary>
    public class JsonIndentationStrategy : DefaultIndentationStrategy
    {
        /// <summary>
        /// Creates a new JsonIndentationStrategy.
        /// </summary>
        public JsonIndentationStrategy()
        {
        }

        /// <summary>
        /// Creates a new JsonIndentationStrategy and initializes the settings using the text editor options.
        /// </summary>
        public JsonIndentationStrategy(TextEditorOptions options)
        {
            this.IndentationString = options.IndentationString;
        }

        string indentationString = "\t";

        /// <summary>
        /// Gets/Sets the indentation string.
        /// </summary>
        public string IndentationString
        {
            get { return indentationString; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Indentation string must not be null or empty");
                indentationString = value;
            }
        }

        /// <summary>
        /// Performs indentation using the specified document accessor.
        /// </summary>
        /// <param name="document">Object used for accessing the document line-by-line</param>
        /// <param name="keepEmptyLines">Specifies whether empty lines should be kept</param>
        public void Indent(IDocumentAccessor document, bool keepEmptyLines)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            IndentationSettings settings = new IndentationSettings();
            settings.IndentString = this.IndentationString;
            settings.LeaveEmptyLines = keepEmptyLines;

            IndentationReformatter r = new IndentationReformatter();
            r.Reformat(document, settings);
        }

        /// <inheritdoc cref="IIndentationStrategy.IndentLine"/>
        public override void IndentLine(TextDocument document, DocumentLine line)
        {
            int lineNr = line.LineNumber;
            TextDocumentAccessor acc = new TextDocumentAccessor(document, lineNr, lineNr);
            Indent(acc, false);

            string t = acc.Text;
            if (t.Length == 0)
            {
                // use AutoIndentation for new lines in comments / verbatim strings.
                base.IndentLine(document, line);
            }
        }

        /// <inheritdoc cref="IIndentationStrategy.IndentLines"/>
        public override void IndentLines(TextDocument document, int beginLine, int endLine)
        {
            Indent(new TextDocumentAccessor(document, beginLine, endLine), true);
        }
    }
}