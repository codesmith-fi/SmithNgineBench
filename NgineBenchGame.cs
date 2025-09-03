using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Codesmith.SmithNgine.Gfx;

using Codesmith.SmithNgine.Smith3D.Primitives;
using Codesmith.SmithNgine.Smith3D.Renderer;
using Codesmith.SmithNgine.Smith3D.Renderer.RenderEffect;
using Codesmith.SmithNgine.View;

/// <summary>
/// Test 
/// </summary>
public class NgineBenchGame : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Scene3D scene3D;
    private Texture2D testPolygonTexture1;
    private Texture2D testPolygonTexture2;
    private Texture2D textureLine;
    private Renderer3D renderer3D;
    private Object3D testObject1;
    private Object3D testObject2;
    private Object3D testObject3;

    BasicEffectParameters basicEffectParameters;
    BasicTextureEffectParameters basicTextureEffectParameters;
    LitTextureAmbientDiffuseEffectParameters litAmbientDiffuseEffectParameters;

    public class GameObjectPolygonData
    {
        public Vertex3D VertexA { get; set; }
        public Vertex3D VertexB { get; set; }
        public Vertex3D VertexC { get; set; }
        public Texture2D Texture { get; set; }
        public Color Color { get; set; } = Color.White;

        public GameObjectPolygonData(Vertex3D a, Vertex3D b, Vertex3D c)
        {
            VertexA = a;
            VertexB = b;
            VertexC = c;
        }

        public GameObjectPolygonData(Vertex3D a, Vertex3D b, Vertex3D c, Color color)
        {
            VertexA = a;
            VertexB = b;
            VertexC = c;
            Color = color;
        }

        public GameObjectPolygonData(Vertex3D a, Vertex3D b, Vertex3D c, Texture2D texture)
        {
            VertexA = a;
            VertexB = b;
            VertexC = c;
            Texture = texture;
        }

        public GameObjectPolygonData(Vertex3D a, Vertex3D b, Vertex3D c, Texture2D texture, Color color)
        {
            VertexA = a;
            VertexB = b;
            VertexC = c;
            Texture = texture;
            Color = color;
        }
    };

    public static readonly GameObjectPolygonData[] testObjectPolygons =
    [
        // FRONT (+Z)
        new GameObjectPolygonData(
                new Vertex3D(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 1)),
                new Vertex3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 1)),
                new Vertex3D(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 0)), Color.Red),
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 0)),
            new Vertex3D(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 0)),
            new Vertex3D(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 1)), Color.Green),

        // BACK (–Z)
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(0, 1)),
            new Vertex3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1, 1)),
            new Vertex3D(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1, 0))),
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(1, 0)),
            new Vertex3D(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(0, 0)),
            new Vertex3D(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(0, 1))),

        // LEFT (–X)
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1)),
            new Vertex3D(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(1, 1)),
            new Vertex3D(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1, 0))),
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(1, 0)),
            new Vertex3D(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 0)),
            new Vertex3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1))),

        // RIGHT (+X)
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(0, 1)),
            new Vertex3D(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 1)),
            new Vertex3D(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0))),
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0)),
            new Vertex3D(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(0, 0)),
            new Vertex3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(0, 1))),

        // TOP (+Y)
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 1)),
            new Vertex3D(new Vector3(0.5f, 0.5f, 0.5f), new Vector2(1, 1)),
            new Vertex3D(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0))),
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(0.5f, 0.5f, -0.5f), new Vector2(1, 0)),
            new Vertex3D(new Vector3(-0.5f, 0.5f, -0.5f), new Vector2(0, 0)),
            new Vertex3D(new Vector3(-0.5f, 0.5f, 0.5f), new Vector2(0, 1))),

        // BOTTOM (–Y)
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1)),
            new Vertex3D(new Vector3(0.5f, -0.5f, -0.5f), new Vector2(1, 1)),
            new Vertex3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 0))),
        new GameObjectPolygonData(
            new Vertex3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector2(1, 0)),
            new Vertex3D(new Vector3(-0.5f, -0.5f, 0.5f), new Vector2(0, 0)),
            new Vertex3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0, 1)))
        ];

    public NgineBenchGame()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        graphics = new GraphicsDeviceManager(this);

        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.IsFullScreen = false;
        graphics.SynchronizeWithVerticalRetrace = true;
        IsFixedTimeStep = false;
        graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        testPolygonTexture1 = Content.Load<Texture2D>("Textures/pillar_texture");
        testPolygonTexture2 = Content.Load<Texture2D>("Textures/dogs1");
        renderer3D = new Renderer3D(GraphicsDevice);

        testObject1 = CreateCube(testPolygonTexture1, 50.0f, new Vector3(0, 0, 200));
        testObject2 = CreateCube(testPolygonTexture1, 100.0f, new Vector3(100, 100, 200));
        testObject3 = CreateCube(testPolygonTexture2, 75.0f, new Vector3(-100, 0, 200));

        var camera3D = new Camera3D(
            new Vector3(0, 0, -100),
            new Vector3(0, 0, 0),
            Vector3.Up,
            MathHelper.ToRadians(45f), // Field of view
            GraphicsDevice.Viewport.AspectRatio, // Aspect ratio
            1f, // Near plane
            1000f // Far plane
        );
        scene3D = new Scene3D(camera3D);

        scene3D.AddObject(testObject1);
        scene3D.AddObject(testObject2);
        scene3D.AddObject(testObject3);

        PointLight pointLight = new PointLight(new Vector3(0, 0, -50), Color.White, 1.0f)
        {
            ConstantAttenuation = 1.0f,
            LinearAttenuation = 0.1f,
            QuadraticAttenuation = 0.01f
        };
        scene3D.AddLight(pointLight);

        // Register supported effects for the 3D renderer
        renderer3D.RegisterEffect<BasicEffectParameters>(
            EffectType.Basic,
            LoadEffectFromFile("Content/Shaders/Basic.mgfxo"));
        renderer3D.RegisterEffect<BasicTextureEffectParameters>(
            EffectType.BasicTexture,
            LoadEffectFromFile("Content/Shaders/BasicTexture.mgfxo"));
        renderer3D.RegisterEffect<LitTextureAmbientDiffuseEffectParameters>(
            EffectType.LitTextureAmbientDiffuse,
            LoadEffectFromFile("Content/Shaders/LitTextureAmbientDiffuse.mgfxo"));

        testObject1.SetEffect(EffectType.BasicTexture);
        testObject2.SetEffect(EffectType.LitTextureAmbientDiffuse);
        testObject3.SetEffect(EffectType.Basic);

        // Set parameters for registered effects
        basicEffectParameters = new BasicEffectParameters() { };
        renderer3D.ApplyParameters(EffectType.Basic, basicEffectParameters);
        basicTextureEffectParameters = new BasicTextureEffectParameters();
        renderer3D.ApplyParameters(EffectType.BasicTexture, basicTextureEffectParameters);
        litAmbientDiffuseEffectParameters = new LitTextureAmbientDiffuseEffectParameters()
        {
            AmbientColor = Color.White,
            AmbientIntensity = 0.50f,
            DiffuseColor = Color.White,
            DiffuseIntensity = 1.0f,
            LightDirection = new Vector3(0f, 0f, 1f)
        };
        renderer3D.ApplyParameters(EffectType.LitTextureAmbientDiffuse, litAmbientDiffuseEffectParameters);
    }

    // Load effect from compiled (DX11) .mgfxo file
    private Effect LoadEffectFromFile(string effectName)
    {
        byte[] shaderBytecode = System.IO.File.ReadAllBytes(effectName);
        return new Effect(GraphicsDevice, shaderBytecode);
    }

    protected override void Update(GameTime gameTime)
    {
        // Check for exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        KeyboardState keyState = Keyboard.GetState();
        // Check for toggle fullscreen
        if (keyState.IsKeyDown(Keys.F10))
        {
            graphics.ToggleFullScreen();
        }
        /*
        else if (keyState.IsKeyDown(Keys.NumPad8))
        {
            renderer.Camera.Position -= new Vector2(0, 10);
        }
        else if (keyState.IsKeyDown(Keys.NumPad2))
        {
            renderer.Camera.Position += new Vector2(0, 10);
        }
        else if (keyState.IsKeyDown(Keys.NumPad4))
        {
            renderer.Camera.Position -= new Vector2(10, 0);
        }
        else if (keyState.IsKeyDown(Keys.NumPad6))
        {
            renderer.Camera.Position += new Vector2(10, 0);
        }
        else if (keyState.IsKeyDown(Keys.NumPad7))
        {
            renderer.Camera.Rotation -= 0.1f;
        }
        else if (keyState.IsKeyDown(Keys.NumPad9))
        {
            renderer.Camera.Rotation += 0.1f;
        }
        else if (keyState.IsKeyDown(Keys.NumPad1))
        {
            renderer.Camera.Scale -= 0.1f;
        }
        else if (keyState.IsKeyDown(Keys.NumPad3))
        {
            renderer.Camera.Scale += 0.1f;
        }
*/
        testObject1.Rotate(new Vector3(0.01f, 0, 0.1f));
        testObject2.Rotate(new Vector3(0, 0.1f, 0.01f));
        testObject3.Rotate(new Vector3(0.1f, 0.01f, 0));
        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        //        renderer3D.DebugRenderAxes(scene3D.Camera); // XYZ debug axes as lines
        renderer3D.RenderScene(scene3D);
        base.Draw(gameTime);
    }

    // Create test object from defined test polygondata

    private Object3D CreateCube(Texture2D texture, float sideLength, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        var obj = new Object3D
        {
            Position = position,
            Rotation = rotation,
            Scale = Vector3.One // Default scale
        };

        if (rotation == default(Quaternion))
        {
            obj.Rotation = Quaternion.CreateFromYawPitchRoll(0, 0, 0);
        }

        foreach (var polyData in testObjectPolygons)
        {
            Vertex3D vtx1 = new Vertex3D(polyData.VertexA);
            Vertex3D vtx2 = new Vertex3D(polyData.VertexB);
            Vertex3D vtx3 = new Vertex3D(polyData.VertexC);

            vtx1.Position *= sideLength; // Scale vertices by side length
            vtx2.Position *= sideLength;
            vtx3.Position *= sideLength;

            vtx1.Color = polyData.Color;
            vtx2.Color = polyData.Color;
            vtx3.Color = polyData.Color;

            var polygon = new Polygon3D(vtx1, vtx2, vtx3, texture);
            obj.AddPolygon(polygon);
        }

        return obj;
    }
}
