using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithProperties
{
    class Properties
    {
        static void Main(string[] args)
        {
            Player player = new Player('@', 10, 0);

            Renderer.DrawPlayer(player);
        }
    }
    class Player 
    {
        public char Avatar { get; private set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public Player(char avatar, int posX, int posY) 
        {
            Avatar = avatar;
            PositionX = posX;
            PositionY = posY;
        }
    }

    static class Renderer 
    {
        public static void DrawPlayer(Player player) 
        {
            Console.SetCursorPosition(player.PositionY, player.PositionX);
            Console.Write(player.Avatar);
        }
    }
}
