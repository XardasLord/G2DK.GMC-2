namespace GothicModComposer.Core.Utils.IOHelpers
{
    public static class GothicRegexHelper
    {
        public const string TwoOrMoreSlashes = @"\s*\/{2,}";
        public const string TranslationId = @"\s*(?<TranslationId>{{.+}})";


        /// <summary>
        ///     This regex was created to extract VDF files Filename, Extension and if it's Disabled
        ///     <Filenmame />.<Extension />.<Disabled />
        /// </summary>
        public const string VdfFile = @"(?<Filename>[\w\W]+?)[.](?<VdfExtension>vdf)(?:(?<Disabled>[.][\w\W]+)?)";

        /// <summary>
        ///     This regex was created to match multiline comments: /*  */
        /// </summary>
        public const string MultiLineComment = @"(?<Comment>\/\*[\s\S]*?\*\/)";

        /// <summary>
        ///     This regex was created to match this kind of expressions:
        ///     MILGreetings				= 	"SVM_1_MILGreetings"				;//Niech żyje Król!
        ///     <Who />                      =   "<Identifier />"                     ;//<Dialogue />
        /// </summary>
        public const string SvmPattern =
            @"\s*(?<Who>\w+)\s*=\s*""(?<Identifier>\w+)""\s*;\s*\/\/(?<Dialogue>.*?)[\r\n]";

        /// <summary>
        ///     This regex will match all the functions in script file without args:
        ///     func NAME() {
        ///     COOOOODEEEE...
        ///     }
        ///     func <Func />() {
        ///     <Content />
        ///     }
        /// </summary>
        public const string FunctionPattern = @"func\s+\w+\s+(?<Func>\w+)\s*()[\s\S]+?{(?<Content>[\s\S]*?)[\n]};";

        /// <summary>
        ///     This is Regex for ini files comments
        /// </summary>
        public const string IniCommentRegex = @"(?<Comment>^;.*)";

        /// <summary>
        ///     This regex will extract INI Sections and it's children
        ///     [ENGINE]
        ///     Something=1
        ///     SomethingElse=2
        ///     [<Header />]
        ///     <Attributes />
        /// </summary>
        public const string SectionRegex = @"\[(?<Header>\w+)\]\n(?<Attributes>[\s\S]+?(?![^[]))";

        /// <summary>
        ///     This regex should parse ini attributes or any kind of attributes that present this pattern
        ///     Something=1
        ///     <Key />=<Value />
        /// </summary>
        public const string IniAttributeRegex = @"(?<Key>\w+)\s*=\s*?(?<Value>.*)";

        public static readonly string OptionalTranslationId = $@"(?:{TwoOrMoreSlashes}{TranslationId})?";

        public static readonly string DefaultFieldPattern = @"[^\r\n]+";
        public static readonly string DefaultStringFieldPattern = @"[^\r\n""]+";

        /// <summary>
        ///     This regex was created to match this kind of expressions:
        ///     AI_Output(self,  other ,   "DIA_Pyrokar_Prologue_WARN_00");     //Ostrzegam cię! Kiedyś możesz tego pożałować.
        ///     AI_Output(<Who>, <ToWho />, <Identifier />                 );     //<Dialogue />
        /// </summary>
        public static readonly string AiOutputPattern =
            $@"AI_Output{Char("(")}{Field("Who")}{Char(",")}{Field("ToWho")}{Char(",")}{StringField("Identifier")}{Char(")")};{EndLineComment("Dialogue")}";

        /// <summary>
        ///     This is Regex for lines containing creation of Journal Log Topics
        ///     Log_CreateTopic(HARAD_QUEST, LOG_MISSION);
        ///     Log_CreateTopic(<Topic />, LOG_MISSION);
        /// </summary>
        public static readonly string JournalCreateTopic =
            $@"Log_CreateTopic{Char("(")}{Field("Topic")}{Char(",")}{Field("State")}{Char(")")};{OptionalTranslationId}";

        /// <summary>
        ///     This is Regex for lines containing Journal Log entries
        ///     B_LogEntry|Log_AddEntry(HARAD_QUEST, "Harad gave me a new quest!");
        ///     B_LogEntry(<Topic />, <Log />);
        ///     Log_AddEntry(<Topic />, <Log />);
        /// </summary>
        public static readonly string JournalLogEntry =
            $@"(?<Type>B_LogEntry|Log_AddEntry){Char("(")}{StringField("Topic")}{Char(",")}{StringField("Log")}{Char(")")};{OptionalTranslationId}";

        /// <summary>
        ///     This is Regex for lines adding new dialogue options in scripts
        ///     Info_AddChoice(some_dialogue_instance, "So what you wanna do next?", new_dialogue_function);
        ///     Info_AddChoice(<Group />, <ChoiceText />, <Choice />);
        ///     <Group /> - It's the name of the dialogue group - like initial dialogue function
        ///     <ChoiceText /> - ONLY IN QUOTES! - It's the text that will be displayed to the player in dialogue box to select
        ///     <Choice /> - It's the name of the function that will be used as next dialogue
        /// </summary>
        public static readonly string InfoAddChoice =
            $@"(?<Type>Info_AddChoice){Char("(")}{Field("Group", @"\w+")}{Char(",")}{Char("\"")}{Field("ChoiceText", ".*?")}{Char("\"")}{Char(",")}{Field("Choice", @"\w+")}{Char(")")};{OptionalTranslationId}";

        /// <summary>
        ///     This is Regex for extracting written document lines
        ///     Doc_PrintLine(some_dialogue_instance, "So what you wanna do next?", new_dialogue_function);
        ///     Doc_PrintLine(<Type />, <Translation />, <Choice />);
        /// </summary>
        public static readonly string DocumentEntries =
            $@"(?<Type>Doc_PrintLines|Doc_PrintLine){Char("(")}{Field("First", ".*?")}{Char(",")}{Field("Second", ".*?")}{Char(",")}{StringField("Translation")}{Char(")")};{OptionalTranslationId}";

        /// <summary>
        ///     This regex finds all methods that contains translation methods with optional TranslationId
        /// </summary>
        public static readonly string TranslationRegex =
            $@"(?<Type>Info_AddChoice|B_LogEntry|Log_AddEntry|Doc_PrintLine|Doc_PrintLines).*;{OptionalTranslationId}";

        public static readonly string TranslationRegexNew =
            $@"(?<TranslationLine>.*;){OptionalTranslationId}";

        /// <summary>
        ///     This regex finds all assignments in Instances that can contain text: name, description and text[n]
        /// </summary>
        public static readonly string AttributeRegexWithTranslation =
            $@"\b(?<Key>name|description|text\[\d\])\s*=\s*\""(?<Value>.+?)\""\s*;{OptionalTranslationId}";

        /// <summary>
        ///     This is Regex for extracting const values
        ///     const string QUEST_LOG = "Some quest log...";
        ///     const <ConstType /> <ConstName /> = <Value />;
        /// </summary>
        public static readonly string Const =
            $@"const\s+(?<ConstType>\w+)\s+(?<ConstName>\w+)\s+=\s+(?<Value>.+)\s*;{OptionalTranslationId}";

        /// <summary>
        ///     This is Regex for extracting const sring values
        ///     const string QUEST_LOG = "Some quest log...";
        ///     const string <ConstName /> = <Value />;
        /// </summary>
        public static readonly string ConstString =
            $@"const\s+string\s+(?<ConstName>\w+)\s*=\s*\""(?<Value>.+)\""\s*;{OptionalTranslationId}";

        /// <summary>
        ///     This is Regex for extracting DialogueInstance blocks
        ///     instance XARDAS_XXX(C_INFO){
        ///     someData = 1;
        ///     Identifier = some_function;
        ///     };
        ///     instance <Name />(C_INFO){
        ///     <Content />
        ///     };
        /// </summary>
        public static readonly string DialogueInstance = Instance("C_INFO");

        /// <summary>
        ///     This is Regex for extracting Npc Defaults blocks
        ///     instance XARDAS_XXX(NPC_DEFAULT){
        ///     someData = 1;
        ///     Identifier = some_function;
        ///     };
        ///     instance <Name />(NPC_DEFAULT){
        ///     <Content />
        ///     };
        /// </summary>
        public static readonly string NpcInstance =
            Instance("NPC_DEFAULT");

        /// <summary>
        ///     This is Regex for extracting Item instances blocks
        ///     instance SOME_ITEM(C_ITEM){
        ///     someData = 1;
        ///     Identifier = some_function;
        ///     };
        ///     instance <Name />(C_ITEM){
        ///     <Content />
        ///     };
        /// </summary>
        public static readonly string ItemInstance =
            Instance("C_ITEM");

        /// <summary>
        ///     This is Regex for extracting Npc Defaults blocks
        ///     instance XARDAS_XXX(NPC_DEFAULT){
        ///     someData = 1;
        ///     Identifier = some_function;
        ///     };
        ///     instance <Name />(NPC_DEFAULT){
        ///     <Content />
        ///     };
        /// </summary>
        public static readonly string SvmInstance = Instance("C_SVM");

        /// <summary>
        ///     This regex should parse ini attributes or any kind of attributes that are ending with semicolon
        ///     Something = 1;
        ///     <Key />=<Value />;
        /// </summary>
        public static readonly string AttributeRegex = $@"(?<Key>\S+)\s*=\s*(?<Value>.*);{OptionalTranslationId}";

        public static string AnyInstance =>
            $@"(instance)\s+{Field("Name", "\\w+")}{Char("(")}{Field("Type", "\\w+")}{Char(")")}[\S\s]*?{Char("{")}{Field("Content", "[\\s\\S]*?")}[\n]}};";

        public static string AnyPrototype =>
            $@"prototype\s+{Field("Name", "\\w+")}{Char("(")}{Field("Type", "\\w+")}{Char(")")}[\S\s]*?{Char("{")}{Field("Content", "[\\s\\S]*?")}[\n]}};";

        public static string Field(string name, string pattern = null) =>
            $@"\s*(?<{name}>{pattern ?? DefaultFieldPattern})";

        public static string StringField(string name, string pattern = null) =>
            $@"\s*""?(?<{name}>{pattern ?? DefaultStringFieldPattern})?""?";

        public static string Char(string character) => $@"\s*[{character}]";
        public static string EndLineComment(string name) => $@"{TwoOrMoreSlashes}[^\S\r\n]*(?<{name}>[^\r\n]*)";

        public static string Instance(string name) =>
            $@"instance\s+{Field("Name", "\\w+")}{Char("(")}\s*{name}{Char(")")}[\S\s]*?{Char("{")}{Field("Content", "[\\s\\S]*?")}[\n]}};";
    }
}