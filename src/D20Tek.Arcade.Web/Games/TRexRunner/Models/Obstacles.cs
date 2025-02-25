namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class Obstacles
{
    private readonly Random _rnd = new();
    private readonly List<Obstacle> _obstacles = new();

    public void Move(GameState state)
    {
        for (int i = _obstacles.Count - 1; i >= 0; i--)
        {
            _obstacles[i].Move();

            // Remove obstacles that go off-screen
            if (_obstacles[i].Bounds.X < -_obstacles[i].Bounds.Width)
            {
                _obstacles.RemoveAt(i);
                state.IncrementScore(1);
            }
        }
    }

    public void GenerateObstacles(GameState state)
    {
        var gameWidth = state.Layout.Viewport.Width;
        if (_obstacles.Count == 0 || (gameWidth - _obstacles.Last().Bounds.X) > _rnd.Next(300, 550))
        {
            var rndType = _rnd.Next(0, 4);
            _obstacles.Add(new(state, rndType));
        }
    }

    public IReadOnlyList<Obstacle> ToList() => _obstacles;

    public Obstacle? First() => _obstacles.FirstOrDefault();
}
