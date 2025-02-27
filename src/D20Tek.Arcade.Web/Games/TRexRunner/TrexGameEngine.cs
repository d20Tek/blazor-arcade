﻿using D20Tek.Arcade.Web.Games.TRexRunner.Models;

namespace D20Tek.Arcade.Web.Games.TRexRunner;

internal class TrexGameEngine
{
    private const int _defaultRefreshRate = 30;
    private readonly GameState _state;

    public int Level { get; private set; } = 1;

    public int Score => _state.Score;

    public bool GameOver { get; private set; } = false;

    public InputController Input { get; }

    public DinoPlayer Dino { get; }

    public Obstacles Obstacles { get; }

    public TrexGameEngine(GameState state)
    {
        _state = state;
        Input = InputController.Create(this);
        Dino = DinoPlayer.Create(_state);
        Obstacles = Obstacles.Create();
    }

    public async Task GameLoop(Action stateChangedAction)
    {
        while (!GameOver)
        {
            Dino.Move();
            Obstacles.GenerateObstacles(_state);
            Obstacles.Move(_state);

            GameOver = Dino.DetectCollision(Obstacles.ToList());

            stateChangedAction();
            await Task.Delay(_defaultRefreshRate);
        }
    }

    public void UpdateLayout(int width)
    {
        var oldWidth = _state.Layout.Viewport.Width;
        var layoutData = _state.LayoutUpdated(LayoutConstants.LayoutSizeFromWidth(width));
        if (oldWidth != layoutData.Viewport.Width)
        {
            Dino.LayoutUpdated(layoutData);
            Obstacles.LayoutUpdated(layoutData);
        }
    }

    public Task EndGame()
    {
        GameOver = true;
        return Task.CompletedTask;
    }
}
