using GoGame.Models;
using GoGame.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GoGame.Views;

public partial class MainWindow : Window
{
    private readonly int _size = 19;
    private double _squareW;
    private double _squareH;
    private Canvas _canvas;
    private Board _board;
    public Brush color => (_board.MoveCounter % 2 == 0) ? Brushes.Black : Brushes.White;
    public Brush Color { get { return color; } }

    public MainWindow()
    {
        InitializeComponent();
        _board = new(_size);
        _canvas = new Canvas
        {
            Height = 400,
            Width = 400,
            Background = Brushes.Peru,
            Margin = new Thickness(400 / (_size - 1) * 1.5)
        };
        DrawBoard();
        Turn();
    }
    private void DrawBoard()
    {
        DataContext = _board;
        ViewboxBoard.Child = _canvas;

        double cWidth = _canvas.Width;
        double cHeight = _canvas.Height;
        _squareW = cWidth / (_size - 1);
        _squareH = cHeight / (_size - 1);

        // Вертикальные линии
        for (int i = 0; i < _size; i++)
        {
            var yStart = 0;
            var yEnd = cHeight;
            var x = i * _squareW;
            Line line = new() { X1 = x, X2 = x, Y1 = yStart, Y2 = yEnd, Stroke = Brushes.Black };
            _canvas.Children.Add(line);
        }

        // Горизонтальные линии
        for (int i = 0; i < _size; i++)
        {
            var xStart = 0;
            var xEnd = cWidth;
            var y = i * _squareH;

            Line line = new() { X1 = xStart, X2 = xEnd, Y1 = y, Y2 = y, Stroke = Brushes.Black };
            _canvas.Children.Add(line);
        }

        // Разметка по столбцам
        for (int i = 0; i < _size; i++)
        {
            char letter = (char)('A' + i);

            TextBlock label = new()
            {
                Text = letter.ToString(),
                FontSize = 12 + _squareH * 0.1,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };

            label.Measure(new System.Windows.Size(_canvas.Height, _canvas.Width));
            Canvas.SetLeft(label, i * _squareW - label.DesiredSize.Width / 2);
            Canvas.SetTop(label, -_squareH * 1.4);
            _canvas.Children.Add(label);
        }

        // Разметка по строкам
        for (int i = 0; i < _size; i++)
        {
            TextBlock label = new()
            {
                Text = (_size - i).ToString(),
                FontSize = 12 + _squareW * 0.1,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black

            };

            label.Measure(new Size(_canvas.Height, _canvas.Width));
            Canvas.SetLeft(label, -_squareW * 1.4);
            Canvas.SetTop(label, i * _squareH - label.DesiredSize.Height / 2);
            _canvas.Children.Add(label);
        }
    }

    private void Turn()
    {
        _canvas.MouseLeftButtonDown += AddStone;
    }

    private void AddStone(object sender, MouseButtonEventArgs e)
    {
        Point coords = e.GetPosition(_canvas);

        int xIndex = (int)Math.Round(coords.X / _squareW);
        int yIndex = (int)Math.Round(coords.Y / _squareH);

        double xMod = coords.X % _squareW;
        double yMod = coords.Y % _squareH;

        if (xIndex >= 0 && xIndex < _size && yIndex >= 0 && yIndex < _size && _board[xIndex, yIndex] == null &&
            (Math.Min(xMod, yMod) < _squareW * 0.2 || Math.Max(xMod, yMod) > _squareW * 0.8))
        {
            Stone stone = new(color == Brushes.Black ? StoneColor.Black : StoneColor.White);
            _board[xIndex, yIndex] = stone;
            _board.MoveCounter += 1;
            if (TurnStatus.Foreground == Brushes.Black)
                TurnStatus.Foreground = Brushes.White;
            else
                TurnStatus.Foreground = Brushes.Black;
        }
        ViewboxBoard.Child = null;
        RefreshBoard();
    }
    private void RefreshBoard()
    {
        _canvas.Children.Clear();

        ViewboxBoard.Child = _canvas;
        DrawBoard();
        for (int x = 0; x < _size; ++x)
        {
            for (int y = 0; y < _size; ++y)
            {
                if (_board[x, y] != null)
                {
                    Engine.RemoveCapturedStones(_board, _board[x, y]);
                    int coordX = (int)(x * _squareW - _squareW * 0.75 / 2);
                    int coordY = (int)(y * _squareH - _squareH * 0.75 / 2);
                    Ellipse circle = new()
                    {
                        Width = _squareW * 0.75,
                        Height = _squareW * 0.75,
                        StrokeThickness = 1
                    };
                    if (_board[x, y].Color == StoneColor.Black)
                        circle.Fill = Brushes.Black;
                    else
                        circle.Fill = Brushes.White;
                    Canvas.SetLeft(circle, coordX);
                    Canvas.SetTop(circle, coordY);
                    _canvas.Children.Add(circle);
                }
            }
        }
    }
    private void EndGame(object sender, RoutedEventArgs e)
    {
        var (blackScore, whiteScore) = _board.ScoringPoints();
        string message = $"Игра окончена! \nЧерные: {blackScore} очков \nБелые: {whiteScore} очков \nПобедили:" +
                                                     $"{(blackScore > whiteScore ? " черные" : " белые")}";
        MessageBox.Show(message, "Конец партии", MessageBoxButton.OK);
        //Обнуление доски и ее отрисовка заново

        _board = new(_size);
        
        _canvas = new Canvas
        {
            Height = 400,
            Width = 400,
            Background = Brushes.Peru,
            Margin = new Thickness(400 / (_size - 1) * 1.5)

        };
        TurnStatus.Foreground = Brushes.Black;
        DrawBoard();
        Turn();
    }
}
