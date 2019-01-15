using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WIreWorldWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int CELLCOUNT = 40;
        public const int CELLSIZE = 20;
        public Brush GRIDCOLOR = Brushes.Gray;
        public Brush BACKGROUND = Brushes.DarkGray;
        public CellTypes currentMode = CellTypes.Conductor;
        CellTypes[,] gridA;
        CellTypes[,] gridB;
        CellTypes[,] selectedGrid;
        WPFDraw drawingCanvas;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            drawPanel.Background = BACKGROUND;
            gridA = new CellTypes[CELLCOUNT, CELLCOUNT];
            gridB = new CellTypes[CELLCOUNT, CELLCOUNT];
            selectedGrid = gridA;
            drawingCanvas = new WPFDraw(drawPanel);
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,500);
        }

        public void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            Paint();
        }

        private void Paint()
        {
            clearPanel();
            drawGrid();
            drawCells();
        }
        private void drawGrid()
        {
            for (int i = 0; i < CELLCOUNT + 1; i++)
            {
                drawingCanvas.DrawLine(0, i * CELLSIZE, CELLSIZE * CELLCOUNT, i * CELLSIZE, GRIDCOLOR);
            }

            for (int i = 0; i < CELLCOUNT + 1; i++)
            {
                drawingCanvas.DrawLine(i * CELLSIZE, 0, i * CELLSIZE, CELLCOUNT * CELLSIZE, GRIDCOLOR);
            }
        }
        private void drawCells()
        {
            CellTypes cellType;
            for (int i = 0; i < CELLCOUNT; i++)
            {
                for (int j = 0; j < CELLCOUNT; j++)
                {
                    cellType = selectedGrid[i, j];
                    if (cellType != CellTypes.Empty)
                    {
                        drawingCanvas.drawRectangle( i * CELLSIZE+1, j * CELLSIZE+1, CELLSIZE-2, CELLSIZE-2, getCellColor(cellType));
                    }
                }
            }
        }
        private void clearGrid(CellTypes[,] cells)
        {
            for (int i = 0; i < CELLCOUNT; i++)
            {
                for (int j = 0; j < CELLCOUNT; j++)
                {
                    cells[i, j] = CellTypes.Empty;
                }
            }
        }
        private void calculateNextStep()
        {
            if (selectedGrid == gridA)
            {
                clearGrid(gridB);
                nextGeneration(selectedGrid, gridB);
                selectedGrid = gridB;
            }
            else
            {
                clearGrid(gridA);
                nextGeneration(selectedGrid, gridA);
                selectedGrid = gridA;
            }
        }
        private void nextGeneration(CellTypes[,] oldGrid, CellTypes[,] newGrid)
        {
            CellTypes cellType;
            for (int i = 0; i < CELLCOUNT; i++)
            {

                for (int j = 0; j < CELLCOUNT; j++)
                {
                    cellType = oldGrid[i, j];
                    if (cellType.Equals(CellTypes.ElectronHead))
                        newGrid[i, j] = CellTypes.ElectronTail;
                    else if (cellType.Equals(CellTypes.ElectronTail))
                        newGrid[i, j] = CellTypes.Conductor;
                    else if (cellType.Equals(CellTypes.Conductor))
                        if (getNumberOfNeighbourHeads(i, j) == 1 || getNumberOfNeighbourHeads(i, j) == 2)
                            newGrid[i, j] = CellTypes.ElectronHead;
                        else
                            newGrid[i, j] = CellTypes.Conductor;
                }
            }
        }
        private Brush getCellColor(CellTypes type)
        {
            if (type.Equals(CellTypes.ElectronHead))
                return Brushes.Blue;
            if (type.Equals(CellTypes.ElectronTail))
                return Brushes.Red;
            if (type.Equals(CellTypes.Conductor))
                return Brushes.Yellow;
            return Brushes.White;
        }
        private int getNumberOfNeighbourHeads(int i, int j)
        {
            int sum = 0;

            sum += valOf(i + 1, j) ? 1 : 0;
            sum += valOf(i - 1, j) ? 1 : 0;
            sum += valOf(i, j + 1) ? 1 : 0;
            sum += valOf(i, j - 1) ? 1 : 0;

            sum += valOf(i + 1, j + 1) ? 1 : 0;
            sum += valOf(i - 1, j - 1) ? 1 : 0;
            sum += valOf(i + 1, j - 1) ? 1 : 0;
            sum += valOf(i - 1, j + 1) ? 1 : 0;

            return sum;
        }
        private bool valOf(int i, int j)
        {
            if (i < 0)
                i += CELLCOUNT;
            if (i == CELLCOUNT)
                i %= CELLCOUNT;

            if (j < 0)
                j += CELLCOUNT;
            if (j == CELLCOUNT)
                j %= CELLCOUNT;

            if (selectedGrid[i, j].Equals(CellTypes.ElectronHead))
                return true;
            else
                return false;
        }
        private void clearPanel()
        {
            drawingCanvas.Clear();
        }

        private void btnStepOnClick(object sender, RoutedEventArgs e)
        {
            calculateNextStep();
            Paint();
        }
        private void btnModeOnCLick(object sender, RoutedEventArgs e)
        {
            currentMode = currentMode + 1;
            if (currentMode == (CellTypes)4)
                currentMode = (CellTypes)1;
            btnMode.Content = currentMode.ToString();
        }
        private void btnAllConductorsClick(object sender, RoutedEventArgs e)
        {
            CellTypes cellType;
            for (int i = 0; i < CELLCOUNT; i++)
            {
                for (int j = 0; j < CELLCOUNT; j++)
                {
                    cellType = selectedGrid[i, j];
                    if (!cellType.Equals(CellTypes.Empty))
                    {
                        selectedGrid[i, j] = CellTypes.Conductor;
                    }
                }
            }
            Paint();
        }
        private void btnClearClick(object sender, RoutedEventArgs e)
        {
            clearGrid(selectedGrid);
            Paint();
        }
        private void cbTickerChecked(object sender, RoutedEventArgs e)
        {
            btnMode.IsEnabled = btnClear.IsEnabled = btnAllConductors.IsEnabled = btnStep.IsEnabled = false;
            dispatcherTimer.Start();
        }
        private void cbTickerUnChecked(object sender, RoutedEventArgs e)
        {
            btnMode.IsEnabled = btnClear.IsEnabled = btnAllConductors.IsEnabled = btnStep.IsEnabled = true;
            dispatcherTimer.Stop();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            calculateNextStep();
            Paint();
        }

        private void drawPanelMouseDown(object sender, MouseButtonEventArgs e)
        {
            ToggleCell((int)e.GetPosition(this.drawPanel).Y / CELLSIZE, (int)e.GetPosition(this.drawPanel).X / CELLSIZE);
            Paint();
        }
        private void ToggleCell(int i, int j)
        {
            if (i < CELLCOUNT && j < CELLCOUNT)
            {
                if (selectedGrid[i, j].Equals(currentMode))
                    selectedGrid[i, j] = CellTypes.Empty;
                else if (i < CELLCOUNT && j < CELLCOUNT)
                    selectedGrid[i, j] = currentMode;
            }
        }
    }
}
