using Godot;
using System;
using Godot.Collections;
using System.Linq;
using System.Collections.Generic;

public static class Extensions
    {
        /// <summary>
        /// Load a scene smartly. Takes a path to a node to instanciate.
        /// </summary>
        /// <typeparam name="T">The type to return. T must inherit node.</typeparam>
        /// <param name="path">The path to load.</param>
        /// <returns></returns>
        public static T SmartLoader<T>(string path) where T : Node
        {
            try
            {
                var resource = ResourceLoader.Load(path);
                var packedScene = (PackedScene)resource;
                var instance = (T)packedScene.Instance();
                return instance;
            }
            catch (Exception exception)
            {
                GD.Print($"Exception while smart loading a scene : {exception.Message}");
                throw exception;
            }
        }

        /// <summary>
        /// Search for children of Type T and return them in a new Array.
        /// </summary>
        /// <typeparam name="T">The type to return. T must inherit node.</typeparam>
        /// <param name="node">The node from which we will start searching.</param>
        /// <returns></returns>
        public static Array<T> FindChildrenOfType<T>(this Node node) where T : Node
        {
            var children = node.GetChildren();
            var nodes = new Array<Node>(children);
            var uncasted = nodes.Where(_ => _.GetType() == typeof(T));

            Array<T> result = new Array<T>();
            foreach (var toCast in uncasted)
            {
                result.Add((T)toCast);
            }

            return result;
        }

        /// <summary>
        /// Transform any C# collections to a Godot Array.
        /// </summary>
        /// <param name="collection">Any C# collection implementing ICollection.</param>
        /// <typeparam name="T">An element inheriting Node.</typeparam>
        /// <returns></returns>
        public static Array<T> ToGodotArray<T>(this IEnumerable<T> collection) where T : Node
        {
            var array = new Godot.Collections.Array<T>();

            foreach (var element in collection)
            {
                array.Add(element);
            }

            return array;
        }
    }