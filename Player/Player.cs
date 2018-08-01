using Godot;
using System;

public class Player : KinematicBody2D
{
    private int speed = 100;
	
	private Vector2 direction = new Vector2(0,0);
    private string spriteDirection = "Down";
    private Vector2 placeholder = new Vector2(0,0);


    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

    public void onInput() {
        bool left = Input.IsActionPressed("ui_left");
        bool right = Input.IsActionPressed("ui_right");
        bool up = Input.IsActionPressed("ui_up");
        bool down = Input.IsActionPressed("ui_down");

         if(up) {
            direction.y = -1;
            spriteDirection = "Up"; 
            
        } else if(down) {
            direction.y = 1;
            spriteDirection = "Down";
        } else {
            direction.y = 0;
        }
        
        if(left) { 
            direction.x = -1;
            // Only update to "left/right" if we aren't already accounting for up/down movement.
            // This gives priority to up/down movement (so you're not facing left and moving up, for example)
            if(!up && !down) spriteDirection = "Left";
        } else if(right) {
            direction.x = 1;
            if(!up && !down) spriteDirection = "Right";
        } else {
            direction.x = 0;
        }

       
    }

    public void updateAnimation(string animation) {
         // Update sprite direction based on movement direction
        
        var newAnimation = animation + spriteDirection;

        // Get the AnimationPlayer
        var AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");

        // If our current animation is not the new one, play the new one
        if(AnimationPlayer.CurrentAnimation != newAnimation) {
            AnimationPlayer.Play(newAnimation);
            GD.Print($"Switching to {newAnimation}");
        }
    }

    public void updatePlayer() {

        var motion = direction.Normalized() * speed;
        MoveAndSlide(motion, placeholder);
    }

    public override void _Process(float delta) {
        onInput();
        updatePlayer();

        if(direction.x != 0 || direction.y != 0) {
            updateAnimation("walk");
        } else {
            updateAnimation("idle");
        }
    }
}
