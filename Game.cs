﻿using System;
using System.Collections;
using System.Collections.Generic;
using MultiMinesweeper.Model;

namespace MultiMinesweeper
{
    public class Game
    {
        public string Id { get; set; }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public Player ReconnectedPlayer { get; set; }
        public Player CurrentPlayer { get; set; }
        private const int NumberOfRows = 16;
        public int FirstPlayerMineCounter { get; set; }
        public int SecondPlayerMineCounter { get; set; }
        public int FirstPlayerFlagCounter { get; set; }
        public int SecondPlayerFlagCounter { get; set; }
        public int FirstPlayerCellClicked { get; set; }
        public int SecondPlayerCellClicked { get; set; }
        
        public GameField[][] Field1;
        public GameField[][] Field2; 
        
        public bool Prepare { get; set; }
        public bool InProgress { get; set; }
        public Game()
        {
            InitialiazeOwnField();
            InitialiazeEnemyField();
            Player1 = new Player();
            Player2 = new Player();
            CurrentPlayer = new Player();
        }

        public void NextPlayer()
        {
            if (CurrentPlayer == Player1)
            {
                CurrentPlayer = Player2;
                Console.WriteLine($"Current player is {CurrentPlayer.Name}");
            }
            else
            {
                CurrentPlayer = Player1;
                Console.WriteLine($"Current player is {CurrentPlayer.Name}");
            }
        }
        
        public bool HasPlayer(string connectionId)
        {
            if (Player1 != null && Player1.ConnectionId == connectionId)
            {
                return true;
            }
            if (Player2 != null && Player2.ConnectionId == connectionId)
            {
                return true;
            }
            return false;
        }

        public bool HasPlayerId(long id)
        {
            if (Player1 != null && Player1.PlayerId == id)
            {
                return true;
            }
            if (Player2 != null && Player2.PlayerId == id)
            {
                return true;
            }
            return false;
        }
        
        public GameField[][] InitialiazeOwnField()
        {
            Field1 = new GameField[NumberOfRows][];
            
            for (int i = 0; i < NumberOfRows; i++)
                Field1[i] = new GameField[NumberOfRows];
            
            return Field1;
        }
        
        public GameField[][] InitialiazeEnemyField()
        {
            Field2 = new GameField[NumberOfRows][];
            
            for (int i = 0; i < NumberOfRows; i++)
                Field2[i] = new GameField[NumberOfRows];
            
            return Field2;
        }

        public GameField[][] OpenCell(int row, int cell, GameField[][] Field)
        {
            Field[row][cell].ClickedCell = true;
            
            if(!Field[row][cell].MinedCell)
                Field[row][cell].NumberCell = true;

            if (Field[row][cell].Merged)
                return Field;

            return Field;
        }
        public GameField[][] CountMines(int row, int cell, GameField[][] Field)
        {
            if (row > 0 && cell == 0)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = 0; y < 1; y++)
                    {
                        if (Field[row][cell+1].MinedCell)
                            Field[row][cell].NeighbourCells++;
                    } 
                }
            }
            else if (row > 0 && cell == NumberOfRows-1)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = 0; y < 1; y++)
                    {
                        if (Field[row][cell-1].MinedCell)
                            Field[row][cell].NeighbourCells++;
                    } 
                }
            }
            else if (row == NumberOfRows-1 && cell > 0)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = 0; y < 1; y++)
                    {
                        if (Field[row-1][cell].MinedCell)
                            Field[row][cell].NeighbourCells++;
                    } 
                }
            }
            else if (row == 0 && cell > 0)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = 0; y < 1; y++)
                    {
                        if (Field[row+1][cell].MinedCell)
                            Field[row][cell].NeighbourCells++;
                    } 
                }
            }
            else if (row == NumberOfRows-1 && cell == 0)
            {
                if (Field[row-1][cell+1].MinedCell)
                    Field[row][cell].NeighbourCells++;
                
                if (Field[row-1][cell].MinedCell)
                    Field[row][cell].NeighbourCells++;
                
                if (Field[row][cell+1].MinedCell)
                    Field[row][cell].NeighbourCells++;
            }
            else if(row == NumberOfRows-1 && cell == NumberOfRows-1)
            {
                if (Field[row-1][cell].MinedCell)
                    Field[row][cell].NeighbourCells++;
                
                if (Field[row-1][cell-1].MinedCell)
                    Field[row][cell].NeighbourCells++;
                
                if (Field[row][cell-1].MinedCell)
                    Field[row][cell].NeighbourCells++;
            }
            else if (row == 0 && cell == 0)
            {
                if (Field[row][cell+1].MinedCell)
                    Field[row][cell].NeighbourCells++;
                
                if (Field[row+1][cell].MinedCell)
                    Field[row][cell].NeighbourCells++;
                
                if (Field[row+1][cell+1].MinedCell)
                    Field[row][cell].NeighbourCells++;
            }
            else
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (Field[row + x][cell + y].MinedCell)
                            Field[row][cell].NeighbourCells++;
                    }
                }
            }

            return Field;
        }
        public GameField[][] PlaceMines(int row, int cell, GameField[][] Field)
        {
            Field[row][cell].MinedCell = true;
            Field[row][cell].ClickedCell = true;
            
            return Field;
        }
        
        public bool IsWin(int clickedCellCounter)
        {
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfRows; j++)
                {
                    if (clickedCellCounter == NumberOfRows * NumberOfRows)
                        return true;
                }
            }

            return false;
        }

        public GameField[][] PlaceFlags(int row, int cell, GameField[][] Field)
        {
            Field[row][cell].Merged = true;
            Field[row][cell].MinedCell = false;
            Field[row][cell].ClickedCell = true;

            return Field;
        }
    }
}