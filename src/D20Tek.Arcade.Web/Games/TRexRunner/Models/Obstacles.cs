namespace D20Tek.Arcade.Web.Games.TRexRunner.Models;

internal class Obstacles
{
    private readonly Random _rnd = new();
    private readonly List<Obstacle> _obstacles = new();

    public void Move()
    {
        for (int i = _obstacles.Count - 1; i >= 0; i--)
        {
            _obstacles[i].Move();

            // Remove obstacles that go off-screen
            if (_obstacles[i].Left < -20)
            {
                _obstacles.RemoveAt(i);
            }
        }
    }

    public void GenerateObstacles(int gameWidth)
    {
        if (_obstacles.Count == 0 || (gameWidth - _obstacles.Last().Left) > _rnd.Next(300, 550))
        {

            _obstacles.Add(new(gameWidth, _rnd.Next(1, 4)));
        }
    }

    public IReadOnlyList<Obstacle> ToList() => _obstacles;
}
