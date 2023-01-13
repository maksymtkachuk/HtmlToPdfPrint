using System;
using System.Collections.Generic;
using System.Linq;


namespace TagParser
{
   public class TagParser {
	   	public char openingCharacter = '<';
		public char closingCharacter = '>';
		public char endTagCharacter = '/';

		public List<TagObject> tags = new List<TagObject>();
		public string plainText = "";


        public void ParseText(string text) {

			// Whenever we encounter an opening character, and the previous
			// character wasn't an escape character (\) create a new TagObject
			// and start counting till the closing character is reached

			string textWithNoTags = ""; // the text without the HTML tags
			bool tagInProgress = false; // tells us whether or not we're in the middle of a tag (between < and >)
			bool tagIsClosing = false; // tells us if the tag we're reading right now is closing a group (with </ )
			TagObject tempTagObject = new TagObject(); // temporary object to store tag info in
			Stack<TagObject> tagStack = new Stack<TagObject>(); // stack that holds all tag objects
			List<TagObject> listOfTags = new List<TagObject>(); // final list of tag objects

			int actualTextIndex = 0; // this will only be incremented when a "normal" character is read


			for (int i = 0; i < text.Length; i++) {
				char nextChar;
				if (i < text.Length - 1) nextChar = text[i+1];
				else nextChar = '\0';

				if (!tagInProgress) {
					bool currentCharacterIsOpeningTag = (text[i] == openingCharacter);
					bool nextCharacterIsEndingTag = (nextChar == endTagCharacter);

					if (currentCharacterIsOpeningTag) {
						tagInProgress = true;
						if (nextCharacterIsEndingTag) {
							tagIsClosing = true;

							tagStack.Peek().endIndex = actualTextIndex - 1;
							//tempTagObject.endIndex = actualTextIndex - 1;
						} else {
							// a new tag is happening
							tempTagObject = new TagObject();
							tagStack.Push(tempTagObject);
							tagStack.Peek().startIndex = actualTextIndex;
							//tempTagObject.startIndex = actualTextIndex;
						}
					} else {
						textWithNoTags += text[i];
						actualTextIndex++;
					}
				} else {
					bool currentCharacterIsClosingTag = (text[i] == closingCharacter); 
					if (currentCharacterIsClosingTag) {
						tagInProgress = false;
						if (tagIsClosing) {
							Console.WriteLine($"closing tag for {tempTagObject.contents}, which starts at {tempTagObject.startIndex}");

							TagObject finishedTagObject = tagStack.Pop();
							listOfTags.Add(finishedTagObject);
							tagIsClosing = false;
						} else {
							// we're done reading an opening tag
							// so let's put this on the stack
							//tagStack.Push(tempTagObject);
							Console.WriteLine($"Pushing the tag {tempTagObject.contents} onto the stack");

						}


					} else if (!tagIsClosing) {
						tagStack.Peek().contents += text[i];
						//tempTagObject.contents += text[i];
					}
				}
				
				
			}


			// Console.WriteLine("========");
			// Console.WriteLine($"Raw text: {textWithNoTags}");
			// Console.WriteLine("========");

			// foreach (TagObject tag in listOfTags) {
			// 	tag.WriteInformation();
			// 	Console.WriteLine("========");
			// }

			tags = listOfTags;
			plainText = textWithNoTags;

        }
	} 


    public class TagObject {
        public string contents = "";
        public int startIndex = 0;
        public int endIndex = 0;

		public void WriteInformation() {
			Console.WriteLine($"Name: {Name}");
			Console.WriteLine($"Start index: {startIndex}");
			Console.WriteLine($"End index: {endIndex}");
			Console.WriteLine("Properties:");
			foreach (TagProperty prop in Properties) {
				prop.WriteInformation();
				Console.WriteLine("\t========");
			}
			return;

		}

		public int[] ArrayOfIndices {
			get {
				int[] outVal = new int[(endIndex - startIndex) + 1];

				for (int i = 0; i <= endIndex - startIndex; i++) {
					outVal[i] = startIndex + i;
				}
				
				return outVal;
			}
		}

		public string Name {
			get {
				return contents.Split(' ')[0];
			}
		}

		public List<TagProperty> Properties {
			get {
				if (contents.Split(' ').Length == 1) {
					return new List<TagProperty>();
				}
					
				List<TagProperty> outVal = new List<TagProperty>();
				string[] rawProps = contents.Split(' ');


				for (int g = 1; g < rawProps.Length; g++) {
					outVal.Add(new TagProperty(rawProps[g]));
				}

				return outVal;
			}
		}
    }

    public class TagProperty {
        public string key = "";
        public string value = "";

		public TagProperty(string input) {
			string[] stuff = input.Split('=');
			key = stuff[0];
			value = stuff[1];
		}

		public void WriteInformation() {
			Console.WriteLine($"\tKey: {key}");
			Console.WriteLine($"\tValue: {value}");
			return;
		}
    }
}
