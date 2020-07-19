using System;
using System.Collections.Generic;

namespace HelL.EntityBase.Conversation
{
    //Ultron
    /* How does this work?
     * You can Create your own Conversation Tree with this tool in the CMD.
     * There is a Example Conversation showing one way to do it.
     * 
     */
    class Conversation
    {
        private Dictionary<string, Node> Nodes;
        private string _name;

        private Conversation()
        {
            Nodes = new Dictionary<string, Node> { };
        }
        /// <summary>
        /// Creates a new Conversation with its Name
        /// </summary>
        /// <param name="ParName">Name of the Conversation</param>
        public Conversation(string ParName)
            : this()
        {
            Name = ParName;
        }
        /// <summary>
        /// Name of the Conversation
        /// </summary>
        public string Name
        {
            get => _name;
            private set => _name = value;
        }
        /// <summary>
        /// Returns the Amount of Nodes in this Conversation
        /// </summary>
        public int Count
        {
            get => Nodes.Count;
        }
        /// <summary>
        /// Returns the Dictionary<string, Node> of this Conversation
        /// </summary>
        public Dictionary<string, Node> GetNodes
        {
            get => Nodes;
        }
        /// <summary>
        /// Keyaccessor for the Nodes
        /// </summary>
        /// <param name="key">for example a char I for InputNode,M for MiddleNode or O for OutputNode combined with a number. I0, I1, M0, M2, O1,O0</param>
        /// <returns>The Node</returns>
        public Node this[string key]
        {
            get => Nodes[key];
        }
        /// <summary>
        /// Adds a Node to the Dictionary with its Key
        /// </summary>
        /// <param name="ParKey">the Key</param>
        /// <param name="ParNode">the Node</param>
        /// <returns>the Key of the added Node</returns>
        public string AddNode(string ParKey, Node ParNode)
        {
            Nodes.Add(ParKey, ParNode);
            return ParKey;
        }
        /// <summary>
        /// Adds a Node to the Conversation with the Nodes Name
        /// </summary>
        /// <param name="ParNode">the Node wanted to be added</param>
        /// <returns>the Key/Name of the Node</returns>
        public string AddNode(Node ParNode)
        {
            Nodes.Add(ParNode.Name, ParNode);
            return ParNode.Name;
        }
        /// <summary>
        /// Adds a new InputNode to the Conversation
        /// </summary>
        /// <param name="ParName">Name of the InputNode</param>
        /// <param name="ParNodeText">Text that gets shown when the Node is called</param>
        /// <param name="ParReactionOption">List of ReactionOptions for the Dialog</param>
        /// <param name="ParPointerKey">Keys pointing to the Next Node</param>
        /// <returns>the Key of added Node</returns>
        public string AddInputNode(string ParName, string ParNodeText, List<string> ParReactionOption, params string[] ParPointerKey)
        {
            return AddNode(ParName, new InputNode(ParName, ParNodeText, ParReactionOption, ParPointerKey));
        }
        /// <summary>
        /// Adds a new MiddleNode to the Conversation
        /// </summary>
        /// <param name="ParName">Name of the Node</param>
        /// <param name="ParNodeText">Text that gets shown when the Node is called</param>
        /// <param name="ParReactionOption">List of ReactionOptions for the Dialog</param>
        /// <param name="ParPointerKey">Keys pointing to the Next Node</param>
        /// <returns>the Key of added Node</returns>
        public string AddMiddleNode(string ParName, string ParNodeText, List<string> ParReactionOption, params string[] ParPointerKey)
        {
            return AddNode(ParName, new MiddleNode(ParName, ParNodeText, ParReactionOption, ParPointerKey));
        }
        /// <summary>
        /// Adds a new OutputNode
        /// </summary>
        /// <param name="ParName">Name of the Node</param>
        /// <param name="ParNodeText">Text that gets shown when the Node is called</param>
        /// <returns>the Key of added Node</returns>
        public string AddOutputNode(string ParName, string ParNodeText)
        {
            return AddNode(ParName, new OutputNode(ParName, ParNodeText));
        }
        /// <summary>
        /// Exchanges target Pointer of Target Node with a new Pointer
        /// </summary>
        /// <param name="TargetNode">Node where the Pointer will be set</param>
        /// <param name="targetPointerKey">index of target Pointer</param>
        /// <param name="newPointerKey">new Pointer</param>
        /// <returns>Old Pointer</returns>
        public string SetPointer(string targetNodeKey, string targetPointerKey, string newPointerKey)
        {
            //Missing Check if newPointer Exists (if wanted)
            MiddleNode targetNode = ((MiddleNode)Nodes[targetNodeKey]);
            int targetPointerIndex = targetNode.IndexOf(targetPointerKey);
            string oldPointer = targetNode[targetPointerIndex];
            targetNode[targetPointerIndex] = newPointerKey;
            return oldPointer;
        }
        /// <summary>
        /// Writes all Nodes to the CMD
        /// </summary>
        public void PrintNodes()
        {
            //for (int i = 0; i < NodeKeys.Count; i++)
            //{
            //    Console.WriteLine(Nodes[NodeKeys[i]]);
            //}Outdated Foreach is the Way to go
            foreach (var item in Nodes)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// Resets the UsedFlag of All ReactionOptions
        /// </summary>
        public void Reset()
        {
            foreach (var node in Nodes)
            {
                if(!(node.Value is OutputNode))
                foreach (ReactionOption reactionOption in node.Value.OPs)
                {
                    if(! (reactionOption is null))
                        reactionOption.ResetFlag();
                }
            }
        }
        /// <summary>
        /// Look at this Example to get a quick Understanding of this Module
        /// </summary>
        public void ExampleConversation()
        {
            //a list of ReactionOptions wich is used twice thats why i created it bevor the Conversation
            //a ReactionOption is a String where the First char is a number betweeen 0 - 9 while 0 is going to call the 0-Element of the LinkArray and the 9-Element is calling the 9-Link
            //there is right now only 10 possible next Nodes but given the extra that two ReactionOptions can link the same Node
            List<string> ReactionOptionList0 = new List<string> { "1'My Name is Mia'", "1'Mia's my Name'", "0'None you should care about'", "0'Whats your name?'" };
            //Adds the InputNode "I0" with its Text "'Hello and Welcome to our MoodBar'" the ReactionOptions: new List<string> { "1'Hi'", "0'Hello'", "0'Olla'", "1'Hey'" }
            AddInputNode("I0", "'Hello and Welcome to our MoodBar'", new List<string> { "1'Hi'", "0'Hello'", "0'Olla'", "1'Hey'" },
                //and directly links the next MiddleNode "M0" with the Text "'And who are you ?'" the ReactionOptionlist0 created previously
                AddMiddleNode("M0", "'And who are you ?'", ReactionOptionList0,
                    //and a link to the next two MiddleNodes "M1" & "M2" here we see the second Option to link two Nodes by adding the Node Name "M0" at the End
                    //is it important that M0 exists when linked ? No, but when you run it will be since its throwing an error.
                    AddMiddleNode("M1", "'Wont you tell me your name ?'", new List<string> { "0'Lets start all over again.'" }, "M0"),
                    AddMiddleNode("M2", "'Welcome Mia.'", new List<string> { "0'Nice to be here'" },
                        //a OutputNode does right now Only contain its Name and Text
                        AddOutputNode("O0", "'Why wont you stay?'")
                        )
                    ),
                AddMiddleNode("M3", "'Nice to meet you ...'", ReactionOptionList0,
                    AddMiddleNode("M4", "'Im Alex.'", new List<string> { "0'Thats a nice name.'" }, "M3"),
                    AddMiddleNode("M5", "'So why are you here?'", new List<string> { "0'I want to explore myself.'" },
                        "O0"
                        )
                    )
                );
            //starts the Conversation
            Run();
        }
        /// <summary>
        /// Gets called at the Start of the Conversation
        /// </summary>
        private void StartCon(bool verbose)
        {
            if (verbose)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Start of Conversation " + Name);
            }
        }
        /// <summary>
        /// Gets called at the End of the Conversation
        /// </summary>
        private void EndCon(bool verbose)
        {
            if (verbose)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("End of Conversation " + Name);
            }
        }
        /// <summary>
        /// Starts/Executes the Conversation
        /// </summary>
        /// <param name="StartingNode">Default"I0" but since your designing you own Conversations this is simply where the Programm should start</param>
        /// <param name="EndNode">Default"O0" Checking the First Char of EndNode and if it is the Same the Programm Exists</param>
        /// <returns>false if Exception</returns>
        public bool Run(string StartingNode = "I0", string EndNode = "O0", bool verbose = false)
        {
            StartCon(verbose);
            if (verbose)
                PrintNodes();
            string CurrentNode = StartingNode;
            while (CurrentNode.Substring(0, 1) != EndNode.Substring(0, 1))
            {
                try
                {
                    CurrentNode = this[CurrentNode].Function(verbose);
                    switch (CurrentNode)
                    {
                        case "E0":
                            if (verbose)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Exception Zero: You ran out of Options");
                            }
                            EndCon(verbose);
                            return false;
                        case "E1":
                            if (verbose)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Exception One: Function() of BaseClass Node was called");
                            }
                            EndCon(verbose);
                            return false;
                        default:
                            break;
                    }
                }
                catch (KeyNotFoundException e)
                {
                    throw new KeyNotFoundException("Key Not Found My Friend", e);
                }
            };
            this[CurrentNode].Function(verbose);
            EndCon(verbose);
            return true;
        }
    }

    /// <summary>
    /// Base Class of all Nodes
    /// </summary>
    class Node
    {
        public short ID;
        private string _name;
        private Byte _type;
        protected string NodeText;
        protected List<ReactionOption> ReactionOption;
        private static byte objectCount;

        protected Node(string ParName, string ParNodeText)
        {
            ID = objectCount;
            ++objectCount;
            _name = ParName;
            Type = 15;
            NodeText = ParNodeText;
        }
        /// <summary>
        /// Use Only in Derived Class
        /// </summary>
        /// <param name="ParName"></param>
        /// <param name="ParNodeText"></param>
        /// <param name="ParReactionOption"></param>
        protected Node(string ParName, string ParNodeText, List<string> ParReactionOption)
            : this(ParName, ParNodeText)
        {
            ReactionOption = HelL.EntityBase.Conversation.ReactionOption.ListFactory(ParReactionOption);
        }
        public byte Type
        {
            get => _type;
            protected set => _type = value;
        }
        /// <summary>
        /// returns the Name and the ObjectCount
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Text
        {
            get => NodeText;
        }
        public List<ReactionOption> OPs
        {
            get => ReactionOption;
        }
        public virtual string Function(bool verbose)
        {
            if (verbose)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ToString() + "Function\n");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(NodeText);
            Console.ForegroundColor = ConsoleColor.White;
            return "E1";
        }
        public int Reaction(bool verbose)
        {
            string Input;
            int Choice = -1;
            short FCounter = 0;
            for (int i = 0; i < ReactionOption.Count; i++)
            {
                //Console.WriteLine(ReactionOption[i].Flag);
                if (ReactionOption[i].UsedFlag)//is Flag of ReactionOption is Set
                {
                    ++FCounter;//Increase the FailCounter
                }
                else//Display the ReactionOption
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine((i + 1) + ": " + ReactionOption[i].GetText(false));
                }
            }
            if (FCounter == ReactionOption.Count)
            {
                Console.WriteLine("You went out of Options");
                return -1;
            }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Input = Console.ReadLine();
            while (InputCheck(Input, ref Choice, ref ReactionOption, verbose))
            {
                Input = Console.ReadLine();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            try
            {
                Console.WriteLine(": " + ReactionOption[Choice].GetText(true));
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + Name + "Reaction" + e.StackTrace);
                throw new IndexOutOfRangeException("index was out of range", e);
            }
            if (!int.TryParse(ReactionOption[Choice].GetIndex(), out Choice))
            {
                Choice = -1;
                throw new FormatException("The first Char of a Reactionoption is the Index of wich Option should be picked out of the List");
            }
            return Choice;
        }
        private static bool InputCheck(string Input, ref int Choice, ref List<ReactionOption> reactionOptions, bool verbose = false)
        {
            int MinInd = 0;
            int MaxInd = reactionOptions.Count - 1;
            bool result = true;
            if (int.TryParse(Input, out Choice))
            {
                Choice -= 1;//Cuz INDEX
                //Console.WriteLine("Choice:" + Choice + "MinInd:" + MinInd + "MaxInd" + MaxInd);
                if (MinInd <= Choice && Choice <= MaxInd)
                {
                    if (reactionOptions[Choice].UsedFlag)
                    {
                        if (verbose)
                            Console.WriteLine("Invalid Input -> ReactionoptionFlag: " + (bool)reactionOptions[Choice].UsedFlag);
                        result = true;
                    }
                    else
                    {
                        if (verbose)
                            Console.WriteLine("Valid Input");
                        return false;
                    }
                }
                else
                {
                    result = true;
                }
            }
            Console.WriteLine("Please Choose a valid white Option from Above");
            return result;
        }
        public override string ToString()
        {
            return Name;
        }
    }

    class InputNode : MiddleNode
    {
        public InputNode(string ParName, string ParNodeText, List<string> ParReactionOption, params string[] ParPointerKey)
            : base(ParName, ParNodeText, ParReactionOption, ParPointerKey)
        {
            Type = 0;
        }
        public override string ToString()
        {
            return "InputNode" + Name;
        }
        public new string ToString(bool verbose = true)
        {
            string tmp = "InputNode " + Name + "\n" + NodeText;
            if (verbose)
                for (int i = 0; i < ReactionOption.Count; ++i)
                {
                    tmp += "\n" + ReactionOption[i];
                }
            return tmp;
        }
    }

    class MiddleNode : Node
    {
        private List<string> Pointer;
        /// <summary>
        /// New Middle Node for a Conversation
        /// </summary>
        /// <param name="ParName"></param>
        /// <param name="ParNodeText">Text shown when Node Function gets called</param>
        /// <param name="ParReactionOption">Reaction Options onto the Text</param>
        /// <param name="ParPointerKey">Keys Referenzing existing Nodes in a Conversation</param>
        public MiddleNode(string ParName, string ParNodeText, List<string> ParReactionOption, params string[] ParPointerKey)
            : base(ParName, ParNodeText, ParReactionOption)
        {
            Type = 1;
            Pointer = new List<string> { };
            AddNode(ParPointerKey);
        }
        public string this[int indexer]
        {
            get => Pointer[indexer];
            set => Pointer[indexer] = value;
        }
        public List<string> Pointers
        {
            get => Pointer;
        }
        public int IndexOf(string ParPointerKey)
        {
            try
            {
                return Pointer.IndexOf(ParPointerKey);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.StackTrace
                    + "MiddleNode:IndexOf: Pointer Not Found");
                return -1;
            }
        }
        public void AddNode(string ParKey)
        {
            Pointer.Add(ParKey);
        }
        public void AddNode(string[] ParKeys)
        {
            Pointer.AddRange(ParKeys);
        }
        public override string Function(bool verbose)
        {
            string LocPointer;
            int LocReaction;
            base.Function(verbose);
            LocReaction = Reaction(verbose);
            if (LocReaction == -1)
            {
                return "E0";//Equivalent to MyOwnException List
            }
            LocPointer = Pointer[LocReaction];
            return LocPointer;
        }
        public override string ToString()
        {
            return "MiddleNode " + Name;
        }
        public string ToString(bool verbose = false)
        {
            string tmp = "MiddleNode " + Name + "\n" + NodeText;
            if (verbose)
            {
                for (int i = 0; i < ReactionOption.Count; ++i)
                {
                    tmp += "\n" + ReactionOption[i];
                }
            }
            return tmp;
        }
    }

    class OutputNode : Node
    {
        public OutputNode(string ParName, string ParNodeText)
            : base(ParName, ParNodeText)
        {
            Type = 2;
        }
        public override string ToString()
        {
            return "OutputNode" + Name;
        }
        public string ToString(bool verbose = true)
        {
            return "OutputNode" + Name + "\n" + (verbose?NodeText:"");
        }
    }
    class ReactionOption
    {
        private string _text;
        private bool _flag;
        ReactionOption(string ParText)
        {
            Text = ParText;
            UsedFlag = false;
        }
        /// <summary>
        /// Text of the ReactionOption.
        /// </summary>
        private string Text
        {
            get => _text;
            set => _text = value;
        }
        /// <summary>
        /// Flag providing Info if Reaction was chosen in the Past
        /// </summary>
        public bool UsedFlag
        {
            get => _flag;
            private set => _flag = value;
        }
        /// <summary>
        /// Resets the Flag to false
        /// </summary>
        /// <returns>true if reset</returns>
        public bool ResetFlag()
        {
            return UsedFlag ? !(UsedFlag = false) : false;
        }
        /// <summary>
        /// the Index of the Option shows wich Pointer of the List is picked
        /// </summary>
        /// <returns>the Index of the Option</returns>
        public string GetIndex()
        {
            return Text.Substring(0, 1);
        }
        /// <summary>
        /// Returns the Text of the ReationOption
        /// </summary>
        /// <param name="SetFlag">should the Flag be set</param>
        /// <returns>the Text without the  Index</returns>
        public string GetText(bool SetFlag)
        {
            if (SetFlag)
            {
                UsedFlag = true;
            }
            return Text.Substring(1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>the Text of the ReactionOption</returns>
        public override string ToString()
        {
            return Text;
        }
        /// <summary>
        /// Creates a ReactionOption List out of a String List
        /// </summary>
        /// <param name="ParList">List of Strings Format: [One Digit Index]'[Text]'</param>
        /// <returns>The List of ReactionOptions</returns>
        public static List<ReactionOption> ListFactory(List<string> ParList)
        {
            List<ReactionOption> result = new List<ReactionOption> { };
            for (int i = 0; i < ParList.Count; i++)
            {
                result.Add(new ReactionOption(ParList[i]));
            }
            return result;
        }
        /// <summary>
        /// Operator to perform a cast to string without the Index. Attention Information Loss is Provided
        /// </summary>
        /// <param name="reactionOption">reactionOption wich you want as a string</param>
        public static implicit operator string(ReactionOption reactionOption) => reactionOption.Text;
        /// <summary>
        /// String to ReactioOption Attention needs the ReactionOption Format of [One Digit Index]'[Text]'
        /// </summary>
        /// <param name="s">[One Digit Index]'[Text]'</param>
        public static implicit operator ReactionOption(string s) => new ReactionOption(s);
    }
}
