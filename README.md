# Blocks Puzzle Game

**Unity Version:** 2020.3.15f2
**Target Platform:** Android
**Levels Format:** JSON

In this game, all the levels are created by the **Procuderal Level Creation Algorithm** that we give their inputs.


https://user-images.githubusercontent.com/22244440/221210486-af8e6014-2439-425a-9c91-5f62a39abdc6.mov


**My Procedural Level Generating Algorithm**

The purpose of this algorithm is create Playable Block puzzle game
levels with take only three inputs.

**Inputs**

● Block Count
● Grid size
● Toggle for whether to add Triangle or not.

**Step 1**

In the first step, I choose random cell on the grid as much as the number of blocks. Then I
place the first parts of our blocks in those cells. While doing this, I check and pay attention to
whether we have selected the same cell.

**Step 2**

In the second step, I try to find as many neighbors as the random value I get for each block
type between 0 and (Grid size x Grid Size / Block Count). I add new block parts of that block
type to the cells I can find.

**Step 3**

In the third step, I check if there is any empty cell on the grid after the second step. If there
is a empty cell, I repeat the second step.

**Step 4**

In the fourth step, if triangle pieces are to be added to the puzzle, I identify the positions
where these triangles will be added and then adding the triangles.
When adding triangles, I first choose a random cell. If the block in that cell is suitable for our
conditions, I make that cell a triangle.
**Our Conditions;**

● All blocks on up, down, right and left are cant be same type with selected cell
● If the right and left of the selected block part are of the same type, the direction of
the triangle can be up or down.
● If the up and down of the selected block part are the same type, the direction of the
triangle can be to the right or to the left.
● The block in the opposite direction of the triangle should be of the same type as the
block part we will make it triangle.
If all conditions are met, I make that block an Outward triangle. I add another block part for
the cell we will make a triangle to the other type of block in the direction of the triangle and
make this part an Inward triangle.

The algorithm is at “LevelCreatorObjects.cs” and it is calling from editor script
“LevelCreatorEditor.cs”.

**Editor Tool Usage Video**



https://user-images.githubusercontent.com/22244440/221212079-520e3cf4-b67e-436e-9e39-cdf023518ab6.mp4


**Gameplay Video**



https://user-images.githubusercontent.com/22244440/221210750-d4729bb1-184e-4db1-82d7-4ce665ada8dd.mp4

