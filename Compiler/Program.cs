using System;
using HackCompiler.Hack;
using System.Xml;

namespace Compiler
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			Lexer lexer = new Lexer("// This file is part of www.nand2tetris.org\r\n// and the book \"The Elements of Computing Systems\"\r\n// by Nisan and Schocken, MIT Press.\r\n// File name: projects/10/ArrayTest/Main.jack\r\n\r\n// (identical to projects/09/Average/Main.jack)\r\n\r\n/** Computes the average of a sequence of integers. */\r\nclass Main {\r\n    function void main() {\r\n        var Array a;\r\n        var int length;\r\n        var int i, sum;\r\n\t\r\n\tlet length = Keyboard.readInt(\"HOW MANY NUMBERS? \");\r\n\tlet a = Array.new(length);\r\n\tlet i = 0;\r\n\t\r\n\twhile (i < length) {\r\n\t    let a[i] = Keyboard.readInt(\"ENTER THE NEXT NUMBER: \");\r\n\t    let i = i + 1;\r\n\t}\r\n\t\r\n\tlet i = 0;\r\n\tlet sum = 0;\r\n\t\r\n\twhile (i < length) {\r\n\t    let sum = sum + a[i];\r\n\t    let i = i + 1;\r\n\t}\r\n\t\r\n\tdo Output.printString(\"THE AVERAGE IS: \");\r\n\tdo Output.printInt(sum / length);\r\n\tdo Output.println();\r\n\t\r\n\treturn;\r\n    }\r\n}\r\n");
			lexer.Parse();
			lexer.dump("test_out.xml");
		}
	}
}
