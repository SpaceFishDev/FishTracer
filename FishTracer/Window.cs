using SDL2;
using System;
namespace FishTracer
{
    class Window
    {
        private int _Width;
        private int _Height;
        private string _Title;
        public byte[] _Pixels;
        public int Width 
        { 
            get { return _Width; } 
            set 
            {
                SDL.SDL_SetWindowSize(winPtr, value, _Height); 
                _Width = value;
            } 
        }
        public int Height
        {
            get { return _Height; }
            set
            {
                SDL.SDL_SetWindowSize(winPtr, Height, value);
                _Height = value;
            }
        }
        private IntPtr winPtr;
        private IntPtr rendererPtr;
        private IntPtr texturePtr;

        public string Title 
        { 
            get { return _Title;} 
            set
            {
                _Title = value;
                SDL.SDL_SetWindowTitle(winPtr, _Title);
            }
        }
        public void PutSquare(int x1, int y1, int x2, int y2, int[] col)
        {
            for (int x = x1; x < x2; x++)
            {
                for (int y = y1; y < y2; y++)
                {
                    PutPixel(x, y, col);
                }
            }
        }
        public void PutPixel(int x, int y, int[] color)
        {
            y = Height - y - 1;
            if (x <= 0 || x >= Width || y <= 0 || y >= Height)
            {
                return;
            }
            _Pixels[x * 4 + (y * 4 * (Width )) + 0] = (byte)color[2];
            _Pixels[x * 4 + (y * 4 * (Width )) + 1] = (byte)color[1];
            _Pixels[x * 4 + (y * 4 * (Width )) + 2] = (byte)color[0];
            _Pixels[x * 4 + (y * 4 * (Width ))+ 3] = 255;
        }

        public Window(int w, int h, string title = "window")
        {
            SDL.SDL_Init(SDL2.SDL.SDL_INIT_EVERYTHING);
            _Width = w;
            _Height = h;
            _Title = title;
            KeyEvents = new Dictionary<int, bool>();
            winPtr = SDL.SDL_CreateWindow(title, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, w, h, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL);
            rendererPtr = SDL.SDL_CreateRenderer(winPtr, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            texturePtr = SDL.SDL_CreateTexture(rendererPtr, SDL.SDL_PIXELFORMAT_ARGB8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, w, h);
            _Pixels = new byte[w * h * 4];

        }
        private Dictionary<int, bool> KeyEvents;
        public bool GetKeyPress(int key) => KeyEvents.ContainsKey(key) ? KeyEvents[key] : false;
        public void HandleEvents()
        {
            SDL.SDL_Event ev;
            while (SDL.SDL_PollEvent(out ev) > 0)
            {
                if(ev.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    _Quit = true;
                }
                if
                (
                    ev.type == SDL.SDL_EventType.SDL_KEYDOWN 
                    || ev.type == SDL.SDL_EventType.SDL_KEYUP
                )
                {
                    KeyEvents[(int)ev.key.keysym.sym] = ev.type == SDL.SDL_EventType.SDL_KEYDOWN;
                }
            }
        }
        private bool _Quit;
        public bool Quit
        {
            get => _Quit;
            set 
            {
                if (value && !_Quit)
                {
                    _Quit = true;
                    SDL.SDL_DestroyWindow(winPtr);
                    SDL.SDL_DestroyRenderer(rendererPtr);
                }
            }
        }
        public void Update(bool TextureUpdate = false)
        {
            if (TextureUpdate)
            {
                IntPtr ptr;
                unsafe
                {
                    fixed (byte* p = _Pixels)
                    {
                        ptr = (IntPtr)p;
                        SDL.SDL_UpdateTexture(texturePtr, IntPtr.Zero, ptr, 4 * Width);
                        SDL.SDL_RenderCopy(rendererPtr, texturePtr, IntPtr.Zero, IntPtr.Zero);
                        SDL.SDL_RenderPresent(rendererPtr);
                        SDL.SDL_RenderClear(rendererPtr);
                    }
                }
            }
            HandleEvents();
        }
    }
}