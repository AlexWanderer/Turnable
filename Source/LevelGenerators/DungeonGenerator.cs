﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Utilities;

namespace Turnable.LevelGenerators
{
    // http://www.roguebasin.com/index.php?title=Basic_BSP_Dungeon_generation
    public class DungeonGenerator : IDungeonGenerator
    {
        public List<Chunk> Chunkify(Chunk initialChunk)
        {
            List<Chunk> randomChunks = new List<Chunk>();
            BinaryTree<Chunk> tree = new BinaryTree<Chunk>();
            tree.Root = new BinaryTreeNode<Chunk>(initialChunk);

            RecursivelyChunkFrom(tree.Root);
            CollectChunks(randomChunks, tree.Root);

            return randomChunks;
        }

        private void CollectChunks(List<Chunk> chunks, BinaryTreeNode<Chunk> node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Left == null && node.Right == null) 
            {
                chunks.Add(node.Value);
            }
            CollectChunks(chunks, node.Left);
            CollectChunks(chunks, node.Right);
        }

        public List<Room> PlaceRooms(List<Chunk> chunks)
        {
            List<Room> rooms = new List<Room>();

            foreach (Chunk chunk in chunks)
            {
                rooms.Add(new Room(chunk, Rectangle.BuildRandomRectangle(chunk.Bounds)));
            }

            return rooms;
        }

        private void RecursivelyChunkFrom(BinaryTreeNode<Chunk> parentChunk)
        {
            // TODO: Put the ability to select randomly from an Enum into the RNG?
            Array values = Enum.GetValues(typeof(SplitDirection));
            Random random = new Random();
            SplitDirection randomSplitDirection = (SplitDirection)values.GetValue(random.Next(values.Length));

            List<Chunk> splitChunks = parentChunk.Value.Split(randomSplitDirection, random.Next(2, 11), 2);

            if (splitChunks.Count != 0)
            {
                parentChunk.Left = new BinaryTreeNode<Chunk>();
                parentChunk.Right = new BinaryTreeNode<Chunk>();

                parentChunk.Left.Value = splitChunks[0];
                parentChunk.Right.Value = splitChunks[1];

                RecursivelyChunkFrom(parentChunk.Left);
                RecursivelyChunkFrom(parentChunk.Right);
            }
        }

        public List<Corridor> JoinRooms(List<Room> rooms)
        {
            // TODO: This code feels like it could be improved.

            throw new NotImplementedException();
        }

        public Corridor GetCorridor(Room firstRoom, Room secondRoom)
        {
            Corridor corridor = new Corridor(firstRoom, secondRoom, null);

            if (firstRoom.Bounds.IsTouching(secondRoom.Bounds))
            {
                return null;
            }

            return corridor;
        }

        public Level Generate()
        {
            throw new NotImplementedException();
        }
    }
}
