using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

namespace ScorpEngine
{
    public interface Behaviour {
        void Init();
        void Start();
        void Update();
        void LateUpdate();
    }

    public class Vector2
    {
        public float x, y;
        public Vector2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        public static Vector2 Zero()
        { return new Vector2(0,0); }
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        { return new Vector2(v1.x + v2.x, v1.y + v2.y); }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        { return new Vector2(v1.x - v2.x, v1.y - v2.y); }
        public static Vector2 operator *(Vector2 v, float k)
        { return new Vector2(v.x * k, v.y * k); }
        public static Vector2 operator *(float k, Vector2 v)
        { return new Vector2(v.x * k, v.y * k); }
        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        { return new Vector2(v1.x * v2.x, v1.y * v2.y); }
        public static Vector2 Dot(Vector2 v1, Vector2 v2)
        { return new Vector2(v1.x * v2.x, v1.y * v2.y); }
        public static float Cross(Vector2 v1, Vector2 v2)
        { return v1.x * v2.y - v1.y * v2.x; }
        public static float Magnitude(Vector2 v)
        { return (float)Math.Abs(Math.Sqrt(Math.Pow(v.x, 2)) + Math.Sqrt(Math.Pow(v.y, 2))); }
        public float magnitude(Vector2 v)
        { return (float)Math.Abs(Math.Sqrt(Math.Pow(x, 2)) + Math.Sqrt(Math.Pow(y, 2))); }
        public static Vector2 Normalize(Vector2 v)
        { return new Vector2(v.x * Magnitude(v), v.y * Magnitude(v)); }

        public void Normalize()
        {
            Vector2 nv = Normalize(new Vector2(x,y));
            x = nv.x;
            y = nv.y;
        }
        public void normalized()
        { Normalize(); }

        public override string ToString()
        { return "("+x.ToString()+", "+y.ToString()+")"; }
    }

    public class TextBuffer
    {
        public Vector2 start;
        public Vector2 end;
        public string text;
        public TextBuffer(string txt, Vector2 s, Vector2 e)
        {
            text = txt;
            start = s;
            end = e;
        }

        public bool isWithinTextBounds(int x, int y)
        { return x >= start.x && x <= end.x && y >= start.y && y <= end.y; }

        public string getTextByAddress(int x, int y)
        {
            int localX = x - (int)start.x;
            int localY = y - (int)start.y;
            int lenX = (int)(end.x - start.x) + 1;
            int lenY = (int)(end.y - start.y);

            localY = lenY - localY;
            
            int i = localX * 2 + lenX * 2 * localY;

            if (text.Length >= i + 2)
                return text[i].ToString() + text[i+1].ToString();
            else if (text.Length >= i + 1)
                return text[i].ToString() + " ";
            else
                return "";
        }
    }

    class Engine
    {
        public static Random rnd = new Random();
        
        // Initialize variables
        public static int[,] sceneData = new int[0,0];
        public static int sceneWidth, sceneHeight, viewportWidth, viewportHeight, cameraX, cameraY, hz;
        public static char outOfBounds = 'x';

        // Runtime variables
        public static char _input;
        public static double delta;

        // Basic functions
        public static void ForceClear() { Console.Clear(); }
        public static void Sleep(float seconds) { Thread.Sleep((int)(seconds * 1000f)); }
        public static bool SetPixelSafe(int x, int y, int data)
        { 
            if (x < 0 || x >= sceneData.GetLength(0) || y < 0 || y >= sceneData.GetLength(1))
                return false;
            sceneData[x,y] = data;
            return true;
        }
        public static bool SetPixelSafe(float x, float y, int data)
        { 
            if ((int)x < 0 || (int)x >= sceneData.GetLength(0) || (int)y < 0 || (int)y >= sceneData.GetLength(1))
                return false;
            sceneData[(int)x,(int)y] = data;
            return true;
        }
        public static bool SetPixelSafe(Vector2 v, int data)
        { 
            if ((int)v.x < 0 || (int)v.x >= sceneData.GetLength(0) || (int)v.y < 0 || (int)v.y >= sceneData.GetLength(1))
                return false;
            sceneData[(int)v.x,(int)v.y] = data;
            return true;
        }
        public static void SetTheme(ConsoleColor bg, ConsoleColor fg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
        }

        static List<TextBuffer> texts = new List<TextBuffer>();
        public static void DrawText(string text, int minX, int maxX, int minY, int maxY)
        {
            texts.Add(new TextBuffer(text, new Vector2(minX, minY), new Vector2(maxX, maxY)));
        }
        public static void Log(string message) { Console.WriteLine(message); }
        public static float RandomNum(float start, float end)
        { return (float) rnd.NextDouble() * (end - start) + start; }
        public static int RandomNum(int start, int end)
        { return rnd.Next(start, end - 1); }

        // Core functions
        public static void RenderDisplay(int[,] data, int w, int h, int x, int y, bool refresh = true)
        {
            if (refresh)
                Console.SetCursorPosition(0,0);
            string _out = "";

            for (int _y = data.GetLength(1) / 2 + y + h / 2; _y >= data.GetLength(1) / 2 + y - h / 2; _y--)
            {
                for (int _x = data.GetLength(0) / 2 + x - w / 2; _x < data.GetLength(0) / 2 + x + w / 2; _x++)
                {
                    bool doCont = false;
                    for (int i = 0; i < texts.Count; i++)
                    {
                        TextBuffer text = texts[i];
                        if (text.isWithinTextBounds(_x,_y))
                        {
                            string at = text.getTextByAddress(_x, _y);
                            _out += at;
                            if (at != "")
                                doCont = true;
                            break;
                        }
                    }
                    if (doCont)
                        continue;

                    if (_x < 0 || _x >= data.GetLength(0) || _y < 0 || _y >= data.GetLength(1)) 
                    {
                        _out += outOfBounds + " ";
                        continue;
                    }
                    _out += (char)(data[_x,_y] == 0 ? ' ' : data[_x,_y]) + " ";
                }
                _out += '\n';
            }

            Console.Write(_out);
            texts.Clear();
        }

        // Assembly code
        private static List<Behaviour> classes = new List<Behaviour>();
        public static void GetClasses()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types = assembly.GetTypes().Where(t => typeof(Behaviour).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
                foreach (Type type in types)
                    classes.Add((Behaviour)Activator.CreateInstance(type));
            }
        }

        // Engine code
        private static void Main()
        {
            GetClasses();

            sceneWidth = 10;
            sceneHeight = 10;
            viewportWidth = 10;
            viewportHeight = 10;
            cameraX = 0;
            cameraY = 0;
            hz = 5;

            foreach(Behaviour beh in classes)
            {
                if (beh != null)
                    beh.Init();
            }

            sceneData = new int[sceneWidth, sceneHeight];

            ForceClear();

            foreach(Behaviour beh in classes)
            {
                if (beh != null)
                    beh.Start();
            }

            double deltaEpoch = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            while (true)
            {
                delta = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - deltaEpoch) / 1000f;
                deltaEpoch = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                if (Console.KeyAvailable) _input = Console.ReadKey(intercept: true).KeyChar; else _input = ' ';

                
                foreach(Behaviour beh in classes)
                {
                    if (beh != null)
                        beh.Update();
                }

                RenderDisplay(sceneData, viewportWidth, viewportHeight, cameraX, cameraY);
                Sleep(1f / hz);
                
                foreach(Behaviour beh in classes)
                {
                    if (beh != null)
                        beh.LateUpdate();
                }
            }
        }
    }
}