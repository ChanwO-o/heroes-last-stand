# heroes-last-stand
CS113 Game Development

## How to Create a Wave
There are three enemies you can specify:
 - `waddle`
 - `ice_waddle`
 - `yellow_waddle`
Each corresponds to the enums declared in `LevelManager`, which in turn, holds an array of prefabs.

The format is pretty straightforward: The number of enemies, followed by the type, repeat.

Example:
1 waddle 10 ice_waddle 5 yellow_waddle

Inside `Assets/Resources/Waves`, there is an example file, `test.txt`.
Any future wave txt files should go there.
