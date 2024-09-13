using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _15Puzzle
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        // 空きパネルの行番号と列番号(ゼロベース)
        int blankPanelRow = 3;
        int blankPanelCol = 3;

        // パズルの状態を管理する2次元配列
        Label[,] puzzle = new Label[4, 4];

        // パネルを動かした回数
        int move = 0;

        // 上下左右どちらの方向に移動できるか管理するビット配列（左、上、右、下）
        BitArray directionBits = new BitArray(4);

        public Form1()
        {
            InitializeComponent();
            InitializePuzzle();
        }

        private void InitializePuzzle()
        {
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                int rowCount = i / 4;
                int colCount = i % 4;

                // puzzleプロパティを初期化
                Label pieceLabel = tableLayoutPanel1.GetControlFromPosition(colCount, rowCount) as Label;


                puzzle[rowCount, colCount] = pieceLabel;
            }

            // パズルのシャッフル
            for (int i = 0; i < 1000; i++)
            {

                // 上下左右どちらの方向と入れ替えるかランダムに決定する
                if (blankPanelCol != 0)
                {
                    // 左に移動可能
                    directionBits[0] = true;
                }
                else
                {
                    directionBits[0] = false;
                }

                if (blankPanelRow != 0)
                {
                    // 上に移動可能
                    directionBits[1] = true;
                }
                else
                {
                    directionBits[1] = false;
                }

                if (blankPanelCol != 3)
                {
                    // 右に移動可能
                    directionBits[2] = true;
                }
                else
                {
                    directionBits[2] = false;
                }

                if (blankPanelRow != 3)
                {
                    // 下に移動可能
                    directionBits[3] = true;
                }
                else
                {
                    directionBits[3] = false;
                }

                List<int> movableIndices = new List<int>();
                for (int j = 0; j < directionBits.Count; j++)
                {
                    if (directionBits[j])
                    {
                        movableIndices.Add(j);
                    }
                }

                int direction = movableIndices[random.Next(0, movableIndices.Count)];
                
                switch (direction)
                {
                    // 左と入れ替え
                    case 0:
                        Swap(blankPanelRow, blankPanelCol, blankPanelRow, blankPanelCol - 1);
                        blankPanelCol--;
                        break;

                    // 上と入れ替え
                    case 1:
                        Swap(blankPanelRow, blankPanelCol, blankPanelRow - 1, blankPanelCol);
                        blankPanelRow--;
                        break;

                    // 右と入れ替え
                    case 2:
                        Swap(blankPanelRow, blankPanelCol, blankPanelRow, blankPanelCol + 1);
                        blankPanelCol++;
                        break;

                    // 下と入れ替え
                    case 3:
                        Swap(blankPanelRow, blankPanelCol, blankPanelRow + 1, blankPanelCol);
                        blankPanelRow++;
                        break;
                }
            }

        }

        // パネルの交換
        private void Swap(int label1Row, int label1Col, int label2Row, int label2Col)
        {
            tableLayoutPanel1.SuspendLayout();

            Label piece1 = tableLayoutPanel1.GetControlFromPosition(label1Col, label1Row) as Label;
            Label piece2 = tableLayoutPanel1.GetControlFromPosition(label2Col, label2Row) as Label;

            TableLayoutPanelCellPosition pos1 = tableLayoutPanel1.GetCellPosition(piece1);
            TableLayoutPanelCellPosition pos2 = tableLayoutPanel1.GetCellPosition(piece2);

            tableLayoutPanel1.SetCellPosition(piece1, pos2);
            tableLayoutPanel1.SetCellPosition(piece2, pos1);

            puzzle[label1Row, label1Col] = piece2;
            puzzle[label2Row, label2Col] = piece1;

            tableLayoutPanel1.ResumeLayout();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Label panel = sender as Label;

            // 上下方向を探索し、空きパネルが存在すれば場所を入れ替え
            TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition(panel);
            if ((pos.Row - 1 == blankPanelRow && pos.Column == blankPanelCol)
                || (pos.Row == blankPanelRow && pos.Column - 1 == blankPanelCol)
                || (pos.Row + 1 == blankPanelRow && pos.Column == blankPanelCol)
                || (pos.Row == blankPanelRow && pos.Column + 1 == blankPanelCol))
            {
                Swap(pos.Row, pos.Column, blankPanelRow, blankPanelCol);
                blankPanelRow = pos.Row;
                blankPanelCol = pos.Column;
            }

            move++;
            MoveLabel.Text = $"Move: {move}";

            if (checkCompletion()) {
                MessageBox.Show($"You solved the puzzle in {move} moves!", "Conguratulation");
                Close();
            }
        }

        private bool checkCompletion()
        {
            for (int i = 0; i < puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < (i == puzzle.GetLength(0) - 1 ? puzzle.GetLength(1) - 1  : puzzle.GetLength(1)); j++)
                {
                    if (puzzle[i, j].Text != ((i * puzzle.GetLength(0) + j) + 1).ToString())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
