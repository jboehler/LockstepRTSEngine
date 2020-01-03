﻿using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using RTSLockstep.Agents;
using RTSLockstep.Abilities;
using RTSLockstep.Player;
using RTSLockstep.Environment;
using RTSLockstep.Simulation.LSMath;
using RTSLockstep.Utility;

namespace RTSLockstep.Managers.GameState
{
    public static class SaveManager
    {
        #region Public
        public static void SaveGame(string filename)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            Directory.CreateDirectory("SavedGames");
            char separator = Path.DirectorySeparatorChar;
            string path = "SavedGames" + separator + PlayerManager.GetPlayerName() + separator + filename + ".json";
            using (StreamWriter sw = new StreamWriter(path))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.WriteStartObject();
                    SaveGameDetails(writer);
                    writer.WriteEndObject();
                }
            }
        }

        public static void SavePlayerResources(JsonWriter writer, RawMaterials currentResources)
        {
            if (writer.IsNull())
            {
                return;
            }

            writer.WritePropertyName("Resources");
            writer.WriteStartArray();
            foreach (var pair in currentResources)
            {
                writer.WriteStartObject();
                WriteLong(writer, pair.Key.ToString(), pair.Value.CurrentValue);
                writer.WriteEndObject();
                writer.WriteStartObject();
                WriteLong(writer, pair.Key.ToString() + "_Limit", pair.Value.CurrentLimit);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        public static void SavePlayerRTSAgents(JsonWriter writer, LSAgent[] agents)
        {
            if (writer.IsNull())
            {
                return;
            }
            writer.WritePropertyName("Agents");
            writer.WriteStartArray();
            foreach (LSAgent agent in agents)
            {
                SaveAgentObject(writer, agent);
            }
            writer.WriteEndArray();
        }

        public static void SaveAgentObject(JsonWriter writer, LSAgent agent)
        {
            if (writer == null || agent == null)
            {
                return;
            }

            writer.WriteStartObject();
            agent.SaveDetails(writer);
            Ability[] abilities = agent.GetComponents<Ability>();
            foreach (Ability ability in abilities)
            {
                ability.SaveDetails(writer);
            }
            writer.WriteEndObject();
        }

        public static void WriteString(JsonWriter writer, string name, string entry)
        {
            if (writer == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            //make sure no bracketed values get stored (e.g. Tank(Clone) becomes Tank)
            if (entry.Contains("("))
            {
                writer.WriteValue((entry.Substring(0, entry.IndexOf("("))).Trim());
            }
            else
            {
                writer.WriteValue(entry.Trim());
            }
        }

        public static void WriteInt(JsonWriter writer, string name, int amount)
        {
            if (writer == null)
            {
                return;
            }
            writer.WritePropertyName(name);
            writer.WriteValue(amount);
        }

        public static void WriteUInt(JsonWriter writer, string name, uint amount)
        {
            if (writer == null)
            {
                return;
            }
            writer.WritePropertyName(name);
            writer.WriteValue(amount);
        }

        public static void WriteLong(JsonWriter writer, string name, long amount)
        {
            if (writer == null)
            {
                return;
            }
            writer.WritePropertyName(name);
            writer.WriteValue(amount);
        }

        public static void WriteFloat(JsonWriter writer, string name, float amount)
        {
            if (writer == null)
            {
                return;
            }
            writer.WritePropertyName(name);
            writer.WriteValue(amount);
        }

        public static void WriteBoolean(JsonWriter writer, string name, bool state)
        {
            if (writer == null)
            {
                return;
            }
            writer.WritePropertyName(name);
            writer.WriteValue(state);
        }

        public static void WriteColor(JsonWriter writer, string name, Color color)
        {
            if (writer == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            writer.WriteStartObject();
            writer.WritePropertyName("r");
            writer.WriteValue(color.r);
            writer.WritePropertyName("g");
            writer.WriteValue(color.g);
            writer.WritePropertyName("b");
            writer.WriteValue(color.b);
            writer.WritePropertyName("a");
            writer.WriteValue(color.a);
            writer.WriteEndObject();
        }

        public static void WriteStringArray(JsonWriter writer, string name, string[] values)
        {
            if (writer == null) return;

            writer.WritePropertyName(name);
            writer.WriteStartArray();
            foreach (string v in values)
            {
                writer.WriteValue(v);
            }
            writer.WriteEndArray();
        }

        public static void WriteRect(JsonWriter writer, string name, Rect rect)
        {
            if (writer == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(rect.x);
            writer.WritePropertyName("y");
            writer.WriteValue(rect.y);
            writer.WritePropertyName("width");
            writer.WriteValue(rect.width);
            writer.WritePropertyName("height");
            writer.WriteValue(rect.height);
            writer.WriteEndObject();
        }

        public static void WriteVector(JsonWriter writer, string name, Vector3 vector)
        {
            if (writer == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(vector.x);
            writer.WritePropertyName("y");
            writer.WriteValue(vector.y);
            writer.WritePropertyName("z");
            writer.WriteValue(vector.z);
            writer.WriteEndObject();
        }

        public static void WriteVector2d(JsonWriter writer, string name, Vector2d vector)
        {
            if (writer == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(vector.x);
            writer.WritePropertyName("y");
            writer.WriteValue(vector.y);
            writer.WriteEndObject();
        }

        public static void WriteVector3d(JsonWriter writer, string name, Vector3d vector)
        {
            if (writer == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(vector.x);
            writer.WritePropertyName("y");
            writer.WriteValue(vector.y);
            writer.WritePropertyName("z");
            writer.WriteValue(vector.z);
            writer.WriteEndObject();
        }

        public static void WriteQuaternion(JsonWriter writer, string name, Quaternion quaternion)
        {
            if (writer == null)
            {
                return;
            }

            writer.WritePropertyName(name);
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(quaternion.x);
            writer.WritePropertyName("y");
            writer.WriteValue(quaternion.y);
            writer.WritePropertyName("z");
            writer.WriteValue(quaternion.z);
            writer.WritePropertyName("w");
            writer.WriteValue(quaternion.w);
            writer.WriteEndObject();
        }
        #endregion

        #region Private
        private static void SaveGameDetails(JsonWriter writer)
        {
            SaveLighting(writer);
            SaveTerrain(writer);
            SaveCamera(writer);
          //  SaveResources(writer);
            SavePlayers(writer);
        }

        private static void SaveLighting(JsonWriter writer)
        {
            Sun sun = (Sun)Object.FindObjectOfType(typeof(Sun));
            if (writer == null || sun == null)
            {
                return;
            }

            writer.WritePropertyName("Sun");
            writer.WriteStartObject();

            WriteVector(writer, "Position", sun.transform.position);
            WriteQuaternion(writer, "Rotation", sun.transform.rotation);
            WriteVector(writer, "Scale", sun.transform.localScale);

            writer.WriteEndObject();
        }

        private static void SaveTerrain(JsonWriter writer)
        {
            //needs to be adapted for terrain once if that gets implemented
            Ground ground = (Ground)GameObject.FindObjectOfType(typeof(Ground));
            if (writer == null || ground == null)
            {
                return;
            }

            writer.WritePropertyName("Ground");
            writer.WriteStartObject();

            WriteVector(writer, "Position", ground.transform.position);
            WriteQuaternion(writer, "Rotation", ground.transform.rotation);
            WriteVector(writer, "Scale", ground.transform.localScale);

            writer.WriteEndObject();
        }

        private static void SaveCamera(JsonWriter writer)
        {
            if (writer == null)
            {
                return;
            }

            writer.WritePropertyName("Camera");
            writer.WriteStartObject();

            Transform cameraTransform = Camera.main.transform;
            WriteVector(writer, "Position", cameraTransform.position);
            WriteQuaternion(writer, "Rotation", cameraTransform.rotation);
            WriteVector(writer, "Scale", cameraTransform.localScale);

            writer.WriteEndObject();
        }

        //private static void SaveResources(JsonWriter writer)
        //{
        //    Resource[] resources = GameObject.FindObjectsOfType(typeof(Resource)) as Resource[];
        //    if (writer == null || resources == null)
        //    {
        //        return;
        //    }
        //    writer.WritePropertyName("Resources");
        //    writer.WriteStartArray();

        //    foreach (Resource resource in resources)
        //    {
        //        SaveAgentObject(writer, resource);
        //    }

        //    writer.WriteEndArray();
        //}

        private static void SavePlayers(JsonWriter writer)
        {
            LSPlayer[] commanders = Object.FindObjectsOfType(typeof(LSPlayer)) as LSPlayer[];
            if (writer.IsNull() || commanders.IsNull())
            {
                return;
            }
            writer.WritePropertyName("Players");
            writer.WriteStartArray();

            foreach (LSPlayer commander in commanders)
            {
                writer.WriteStartObject();
                commander.SaveDetails(writer);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
        #endregion
    }
}