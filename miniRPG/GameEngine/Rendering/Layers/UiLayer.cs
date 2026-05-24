using System.Diagnostics;
using System.Drawing.Drawing2D;
using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.Events;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Rendering.Layers;

// PROMPT UiLayer: I need you to implement these features: Every time I gain some xp I need a Statístic Bar (something like in minecraft there is a "bossbar" which
// displays the health of the current boss you are fighting, but in this case it will be an experience bar) to display on the top of the screen with the name
// of the skill and the amount of xp gained, and it should disappear after 4 seconds.
// Also, when I level up I want a message to appear in the middle of the screen with the text "LEVEL UP!" and
// "You are now Level X" where X is the new level number, and this message should have a nice animation and disappear after 2 seconds.
// Please implement these features in the UiLayer class. 
// Keep the structure and architecture as CLOSE to other RenderLayers and my other code! 

public class UiLayer : IRenderLayer
{
    private readonly World _world;
    private bool _isLevelUpMessageVisible;
    private int _newLevelNumber;
    private float _levelUpMessageScale = 0f; // For animation
    private float _levelUpMessageAlpha = 0f; // For animation

    private readonly Font _titleFont = new("Segoe UI", 16, FontStyle.Bold);
    private readonly Font _subtitleFont = new("Segoe UI", 10, FontStyle.Regular);
    
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private double _levelUpMessageEndTime = 0.0;
    private const double LevelUpMessageDurationSeconds = 4.0;

    // --- Experience Bar Fields ---
    private bool _isExperienceBarVisible;
    private double _experienceBarEndTime;
    private SkillType _currentSkill;
    private bool _justLeveledUp = false;
    
    // For animation
    private float _animatedExperience; 
    private float _targetExperience;
    private int _experienceForNextLevel;
    private const float AnimationSpeed = 5.0f;

    // --- Wave Message Fields ---
    private bool _isWaveMessageVisible;
    private int _currentWaveNumber;
    private float _waveMessageScale = 0f;
    private float _waveMessageAlpha = 0f;
    private double _waveMessageEndTime = 0.0;
    private const double WaveMessageDurationSeconds = 4.0;

    // --- Death Message Fields ---
    private bool _isDeathMessageVisible;
    private float _deathMessageScale = 0f;
    private float _deathMessageAlpha = 0f;
    private double _deathMessageEndTime = 0.0;
    private const double DeathMessageDurationSeconds = 3.0;

    public UiLayer(World world)
    {
        _world = world;
        _animatedExperience = 0;
        _targetExperience = 0;
        _experienceForNextLevel = 1; // Avoid division by zero on first frame

        _world.EventBus.Subscribe<LevelUpEvent>(OnLevelUp);
        _world.EventBus.Subscribe<ExperienceGainedEvent>(OnExperienceGained);
        _world.EventBus.Subscribe<EnemyWaveEvent>(OnEnemyWave);
        _world.EventBus.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
    }

    public void Update(float deltaTime)
    {
        // --- Update Timers ---
        if (_isLevelUpMessageVisible && _stopwatch.Elapsed.TotalSeconds >= _levelUpMessageEndTime)
            _isLevelUpMessageVisible = false;

        if (_isWaveMessageVisible && _stopwatch.Elapsed.TotalSeconds >= _waveMessageEndTime)
            _isWaveMessageVisible = false;

        if (_isDeathMessageVisible && _stopwatch.Elapsed.TotalSeconds >= _deathMessageEndTime)
            _isDeathMessageVisible = false;

        if (_isExperienceBarVisible && _stopwatch.Elapsed.TotalSeconds >= _experienceBarEndTime)
            _isExperienceBarVisible = false;

        // --- Animate Level Up Message ---
        if (_isLevelUpMessageVisible)
        {
            var timeSinceDisplay = (_stopwatch.Elapsed.TotalSeconds - (_levelUpMessageEndTime - LevelUpMessageDurationSeconds));
            var fadeTime = 0.5; // 0.5s fade in, 0.5s fade out

            // Fade In
            if (timeSinceDisplay < fadeTime)
                _levelUpMessageAlpha = (float)(timeSinceDisplay / fadeTime);
            // Fade Out
            else if (timeSinceDisplay > LevelUpMessageDurationSeconds - fadeTime)
                _levelUpMessageAlpha = (float)((LevelUpMessageDurationSeconds - timeSinceDisplay) / fadeTime);
            else
                _levelUpMessageAlpha = 1f;
            
            // Simple bounce-in animation
            _levelUpMessageScale = OtherHelpers.Lerp(_levelUpMessageScale, 1f, deltaTime * 10f);
        }
        else
        {
            _levelUpMessageScale = 0f; // Reset when not visible
            _levelUpMessageAlpha = 0f;
        }

        // --- Animate Wave Message ---
        if (_isWaveMessageVisible)
        {
            var timeSinceDisplay = (_stopwatch.Elapsed.TotalSeconds - (_waveMessageEndTime - WaveMessageDurationSeconds));
            var fadeTime = 0.5;

            // Fade In
            if (timeSinceDisplay < fadeTime)
                _waveMessageAlpha = (float)(timeSinceDisplay / fadeTime);
            // Fade Out
            else if (timeSinceDisplay > WaveMessageDurationSeconds - fadeTime)
                _waveMessageAlpha = (float)((WaveMessageDurationSeconds - timeSinceDisplay) / fadeTime);
            else
                _waveMessageAlpha = 1f;
            
            _waveMessageScale = OtherHelpers.Lerp(_waveMessageScale, 1f, deltaTime * 10f);
        }
        else
        {
            _waveMessageScale = 0f;
            _waveMessageAlpha = 0f;
        }

        // --- Animate Death Message ---
        if (_isDeathMessageVisible)
        {
            var fadeTime = 0.5;
            var timeSinceDisplay = (_stopwatch.Elapsed.TotalSeconds - (_deathMessageEndTime - DeathMessageDurationSeconds));

            if (timeSinceDisplay < fadeTime)
                _deathMessageAlpha = (float)(timeSinceDisplay / fadeTime);
            else if (timeSinceDisplay > DeathMessageDurationSeconds - fadeTime)
                _deathMessageAlpha = (float)((DeathMessageDurationSeconds - timeSinceDisplay) / fadeTime);
            else
                _deathMessageAlpha = 1f;

            _deathMessageScale = OtherHelpers.Lerp(_deathMessageScale, 1f, deltaTime * 8f);
        }
        else
        {
            _deathMessageScale = 0f;
            _deathMessageAlpha = 0f;
        }

        // --- Animate Experience Bar ---
        if (_isExperienceBarVisible)
            _animatedExperience = OtherHelpers.Lerp(_animatedExperience, _targetExperience, deltaTime * AnimationSpeed);
    }
    
    public void Render(World world, RenderContext context)
    {
        if (_levelUpMessageAlpha > 0)
        {
            RenderLevelUpMessage(context);
        }

        if (_waveMessageAlpha > 0)
        {
            RenderWaveMessage(context);
        }

        if (_deathMessageAlpha > 0)
        {
            RenderDeathMessage(context);
        }

        if (_isExperienceBarVisible)
        {
            RenderExperienceBar(context);
        }
        
        RenderHealthBar(world, context);
        
    }

    private void RenderLevelUpMessage(RenderContext context)
    {
        var levelUpText = "LEVEL UP!";
        var newLevelText = $"You are now Level {_newLevelNumber}";

        using (var titleFont = new Font("Segoe UI", 16, FontStyle.Bold))
        using (var subtitleFont = new Font("Segoe UI", 10, FontStyle.Regular))
        {
            var titleSize = context.Graphics.MeasureString(levelUpText, titleFont);
            var subtitleSize = context.Graphics.MeasureString(newLevelText, subtitleFont);
            
            var boxWidth = Math.Max(titleSize.Width, subtitleSize.Width) + 40;
            var boxHeight = titleSize.Height + subtitleSize.Height + 20;

            var x = (context.ScreenWidth - boxWidth) / 2;
            var y = 70;

            // Apply scale animation
            var transform = context.Graphics.Transform;
            context.Graphics.TranslateTransform(x + boxWidth / 2, y + boxHeight / 2);
            context.Graphics.ScaleTransform(_levelUpMessageScale, _levelUpMessageScale);
            context.Graphics.TranslateTransform(-(x + boxWidth / 2), -(y + boxHeight / 2));

            var currentAlpha = (int)(_levelUpMessageAlpha * 255);

            // --- Draw Text ---
            var titleX = x + (boxWidth - titleSize.Width) / 2;
            var titleY = y + 10;
            context.Graphics.DrawString(levelUpText, _titleFont, new SolidBrush(Color.FromArgb(currentAlpha, Color.White)), titleX, titleY);

            var subtitleX = x + (boxWidth - subtitleSize.Width) / 2;
            var subtitleY = titleY + titleSize.Height;
            context.Graphics.DrawString(newLevelText, _subtitleFont, new SolidBrush(Color.FromArgb(currentAlpha, Color.White)), subtitleX, subtitleY);
        }
    }

    private void RenderWaveMessage(RenderContext context)
    {
        var waveText = $"WAVE {_currentWaveNumber} SPAWNED!";
        var subtitleText = "Prepare yourself!";

        using (var titleFont = new Font("Impact", 24, FontStyle.Regular))
        using (var subtitleFont = new Font("Segoe UI", 12, FontStyle.Italic))
        {
            var titleSize = context.Graphics.MeasureString(waveText, titleFont);
            var subtitleSize = context.Graphics.MeasureString(subtitleText, subtitleFont);
            
            var boxWidth = Math.Max(titleSize.Width, subtitleSize.Width) + 40;
            var boxHeight = titleSize.Height + subtitleSize.Height + 20;

            var x = (context.ScreenWidth - boxWidth) / 2;
            var y = 150; // Render a bit lower than Level Up msg

            var transform = context.Graphics.Transform;
            context.Graphics.TranslateTransform(x + boxWidth / 2, y + boxHeight / 2);
            context.Graphics.ScaleTransform(_waveMessageScale, _waveMessageScale);
            context.Graphics.TranslateTransform(-(x + boxWidth / 2), -(y + boxHeight / 2));

            var currentAlpha = (int)(_waveMessageAlpha * 255);

            // --- Draw Text ---
            var titleX = x + (boxWidth - titleSize.Width) / 2;
            var titleY = y + 10;
            context.Graphics.DrawString(waveText, titleFont, new SolidBrush(Color.FromArgb(currentAlpha, Color.Red)), titleX, titleY);

            var subtitleX = x + (boxWidth - subtitleSize.Width) / 2;
            var subtitleY = titleY + titleSize.Height;
            context.Graphics.DrawString(subtitleText, subtitleFont, new SolidBrush(Color.FromArgb(currentAlpha, Color.White)), subtitleX, subtitleY);

            context.Graphics.Transform = transform; // Reset transform
        }
    }

    private void RenderDeathMessage(RenderContext context)
    {
        var deathText = "YOU DIED";
        var subtitleText = "Respawning at center...";

        using (var titleFont = new Font("Impact", 36, FontStyle.Regular))
        using (var subtitleFont = new Font("Segoe UI", 14, FontStyle.Italic))
        {
            var titleSize = context.Graphics.MeasureString(deathText, titleFont);
            var subtitleSize = context.Graphics.MeasureString(subtitleText, subtitleFont);
            
            var boxWidth = Math.Max(titleSize.Width, subtitleSize.Width) + 40;
            var boxHeight = titleSize.Height + subtitleSize.Height + 20;

            var x = (context.ScreenWidth - boxWidth) / 2;
            var y = context.ScreenHeight / 2 - boxHeight / 2; // Center horizontally and vertically

            var transform = context.Graphics.Transform;
            context.Graphics.TranslateTransform(x + boxWidth / 2, y + boxHeight / 2);
            context.Graphics.ScaleTransform(_deathMessageScale, _deathMessageScale);
            context.Graphics.TranslateTransform(-(x + boxWidth / 2), -(y + boxHeight / 2));

            var currentAlpha = (int)(_deathMessageAlpha * 255);

            // --- Draw Text ---
            var titleX = x + (boxWidth - titleSize.Width) / 2;
            var titleY = y + 10;
            context.Graphics.DrawString(deathText, titleFont, new SolidBrush(Color.FromArgb(currentAlpha, Color.DarkRed)), titleX, titleY);

            var subtitleX = x + (boxWidth - subtitleSize.Width) / 2;
            var subtitleY = titleY + titleSize.Height;
            context.Graphics.DrawString(subtitleText, subtitleFont, new SolidBrush(Color.FromArgb(currentAlpha, Color.DarkGray)), subtitleX, subtitleY);

            context.Graphics.Transform = transform; // Reset transform
        }
    }

    private void RenderExperienceBar(RenderContext context)
    {
        var barWidth = context.ScreenWidth * 0.4f; // Made it a bit smaller
        var barHeight = 12;
        var x = (context.ScreenWidth - barWidth) / 2;
        var y = 30;
        var cornerRadius = barHeight / 2f;

        // --- Draw Bar Background with Gradient ---
        var backgroundRect = new RectangleF(x, y, barWidth, barHeight);
        using (var path = CreateRoundedRectangle(backgroundRect, cornerRadius))
        using (var brush = new SolidBrush(Color.FromArgb(100, 0, 0, 0))) // Semi-transparent black
        {
            context.Graphics.FillPath(brush, path);
        }

        // --- Draw Progress Bar ---
        var progress = Math.Clamp(_animatedExperience / _experienceForNextLevel, 0f, 1f);
        if (progress > 0)
        {
            var progressWidth = barWidth * progress;
            var progressRect = new RectangleF(x, y, progressWidth, barHeight);
            using (var path = CreateRoundedRectangle(progressRect, cornerRadius))
            using (var brush = new LinearGradientBrush(progressRect, Color.FromArgb(0, 191, 255), Color.FromArgb(0, 128, 255), 90f)) // Blue gradient
            {
                context.Graphics.FillPath(brush, path);
            }
        }
        
        // --- Draw Text with Shadow ---
        var text = $"{_currentSkill} XP: {(int)_animatedExperience} / {_experienceForNextLevel}";
        using (var font = new Font("Segoe UI", 9, FontStyle.Bold))
        {
            var textSize = context.Graphics.MeasureString(text, font);
            var textX = x + (barWidth - textSize.Width) / 2;
            var textY = y + (barHeight - textSize.Height) / 2;

            // Shadow
            context.Graphics.DrawString(text, font, Brushes.Black, textX + 1, textY + 1);
            // Main Text
            context.Graphics.DrawString(text, font, Brushes.White, textX, textY);
        }
    }

    private void OnLevelUp(LevelUpEvent e)
    {
        _isLevelUpMessageVisible = true;
        _newLevelNumber = e.NewLevel;
        _levelUpMessageScale = 0.5f; // Start scale small for bounce-in effect
        _animatedExperience = 0; // On level up, reset the animated value to 0 for the new bar
        _targetExperience = 0;
        _justLeveledUp = true;
        // schedule when the message should stop being visible
        _levelUpMessageEndTime = _stopwatch.Elapsed.TotalSeconds + LevelUpMessageDurationSeconds;
    }

    private void OnEnemyWave(EnemyWaveEvent e)
    {
        _isWaveMessageVisible = true;
        _currentWaveNumber = e.WaveNumber;
        _waveMessageScale = 0.5f; 
        _waveMessageEndTime = _stopwatch.Elapsed.TotalSeconds + WaveMessageDurationSeconds;
    }

    private void OnPlayerDeath(PlayerDeathEvent e)
    {
        _isDeathMessageVisible = true;
        _deathMessageScale = 0.5f;
        _deathMessageEndTime = _stopwatch.Elapsed.TotalSeconds + DeathMessageDurationSeconds;
    }

    private void OnExperienceGained(ExperienceGainedEvent e)
    {
        // If we just leveled up, this XP is the new starting amount.
        if (_justLeveledUp)
        {
            _animatedExperience = 0;
            _justLeveledUp = false;
        }
        // If the bar was hidden, snap the animation start to the previous value.
        else if (!_isExperienceBarVisible)
        {
            _animatedExperience = e.CurrentExperience - e.XpGained;
        }
        
        _isExperienceBarVisible = true;
        _currentSkill = e.Skill;
        _targetExperience = e.CurrentExperience;
        _experienceForNextLevel = e.ExperienceForNextLevel;
        _experienceBarEndTime = _stopwatch.Elapsed.TotalSeconds + 4.0; // Increased visibility time
    }
    
    // NOTE: Find explanation of how this works
    private static GraphicsPath CreateRoundedRectangle(RectangleF rect, float radius)
    {
        var path = new GraphicsPath();
        if (radius <= 0)
        {
            path.AddRectangle(rect);
            return path;
        }
        // To avoid issues with small rectangles
        radius = Math.Min(radius, rect.Width / 2);
        radius = Math.Min(radius, rect.Height / 2);

        path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
        path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
        path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
        path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
        path.CloseFigure();
        return path;
    }
    
    private void RenderHealthBar(World world, RenderContext context)
    {
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<HealthBarComponent>())
                continue;

            var healthBar = e.GetComponent<HealthBarComponent>();
            if (healthBar == null)
                continue;

            // Calculate the filled width based on health percentage
            float healthPercent = (float)healthBar.CurrentHealth / healthBar.MaxHealth;
            int filledWidth = (int)(healthPercent * healthBar.Width);

            // Draw background 
            using var bgBrush = new SolidBrush(healthBar.BackgroundColor);
            context.Graphics.FillRectangle(bgBrush, healthBar.X, healthBar.Y, healthBar.Width, healthBar.Height);
            // Draw foreground (
            var fillBrush = new SolidBrush(healthBar.FillColor);
            context.Graphics.FillRectangle(fillBrush, healthBar.X, healthBar.Y, filledWidth, healthBar.Height);

            // Draw border
            using var borderPen = new Pen(healthBar.BorderColor);
            context.Graphics.DrawRectangle(borderPen, healthBar.X, healthBar.Y, healthBar.Width, healthBar.Height);

            // Draw health text
            if (!healthBar.ShowText)
                continue;
            
            string healthText = $"{healthBar.CurrentHealth}/{healthBar.MaxHealth}";
            using var font = new Font("Arial", healthBar.FontSize, FontStyle.Bold);
            
                var textSize = context.Graphics.MeasureString(healthText, font);
                float textX = healthBar.X + (healthBar.Width - textSize.Width) / 2;
                float textY = healthBar.Y + (healthBar.Height - textSize.Height) / 2;
            
                context.Graphics.DrawString(healthText, font, Brushes.White, textX, textY);
            
        }
    }
    
}