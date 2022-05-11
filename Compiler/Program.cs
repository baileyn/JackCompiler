using System;
using HackCompiler.Hack;
using System.Xml;

namespace Compiler
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Lexer lexer = new Lexer("// This file is part of www.nand2tetris.org\r\n// and the book \"The Elements of Computing Systems\"\r\n// by Nisan and Schocken, MIT Press.\r\n// File name: projects/10/Square/Main.jack\r\n\r\n// (derived from projects/09/Square/Main.jack, with testing additions)\r\n\r\n/** Initializes a new Square Dance game and starts running it. */\r\nclass Main {\r\n    static boolean test;    // Added for testing -- there is no static keyword\r\n                            // in the Square files.\r\n    function void main() {\r\n      var SquareGame game;\r\n      let game = SquareGame.new();\r\n      do game.run();\r\n      do game.dispose();\r\n      return;\r\n    }\r\n\r\n    function void test() {  // Added to test Jack syntax that is not use in\r\n        var int i, j;       // the Square files.\r\n        var String s;\r\n        var Array a;\r\n        if (false) {\r\n            let s = \"string constant\";\r\n            let s = null;\r\n            let a[1] = a[2];\r\n        }\r\n        else {              // There is no else keyword in the Square files.\r\n            let i = i * (-j);\r\n            let j = j / (-2);   // note: unary negate constant 2\r\n            let i = i | j;\r\n        }\r\n        return;\r\n    }\r\n}\r\n");
			lexer.Parse();
			lexer.dump("test_out.xml");
		}
	}
}
