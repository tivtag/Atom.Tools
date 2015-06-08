
The Sprite Atlas Batcher is a simple batch program that uses XML-configuration files to generate an Atom Sprite Database (.sdb) given a set of sprite images.

Additionally the individual sprites are compacted into a single sprite atlas texture. As such only one texture must be loaded into video memory.
The Sprite Database contains information such as in what location each indivual sprite is located in the sprite atlas.

The Sprite Database can also contain Sprite Animations. These are created using the Sprite Tool.

------------------------------------------------------

See ExampleConfig.xml on how to configure the Sprite Atlas Batcher.

Example Link to start the Sprite Atlas Batcher:
"C:\Users\Case\Documents\Visual Studio 2012\Projects\Games\Zelda\Content\BlackCrown_ContentProject\SpriteTool.AtlasBatcher.exe" Content/Configuration/ExampleConfig.xml