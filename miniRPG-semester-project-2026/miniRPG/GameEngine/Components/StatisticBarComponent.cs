namespace miniRPG.GameEngine.Components;


public class StatisticBarComponent
{
    // This ui component will sit in the right corner of the screen
    // NOTE: Try not to break anything
    
    // TODO: Have a sprite of the bar
    public int MaxHealth = 100;
    public int MinHealth = 0;
    public int CurrentHealth = 100;
    public Brush StatBarColor;
    public Image FrameBorder;

    // TODO: Edit Render method to draw only the right part of the bar, depending on the current value
    // TODO: Now you can create for the first time UI entity
    // TODO: MAKE IT WHOLE GENERIC SO ITS NOT JUST FOR HEALTH SO I CAN CREATE MORE STATBARS THAT WILL BE POSSIBLE TO EDIT WITH
    // TODO: EVENTS LATER FOR NOW JUST KEYBOARD SO EVERYTHING SHOULD BE NAMED INSTEAD OF "MaxHealth" this "MaxValue" 
    // TODO: Play with it just set some numbers
    // TODO: Now add system that subtracts and adds health based on the input, make it generic
    // TODO: Use my keyboard helper to just run some test in game for modyfing the value
}