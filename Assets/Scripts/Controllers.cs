using System.Collections.Generic;

class Controllers : IInitializeable, IFrameUpdate, IFixedFrameUpdate, ICleanupable
{
    List<IInitializeable> _initializeables;
    List<IFrameUpdate> _frameUpdates;
    List<IFixedFrameUpdate> _fixedFrameUpdates;
    List<ICleanupable> _cleanupables;

    public Controllers()
    {
        _initializeables = new List<IInitializeable>();
        _frameUpdates = new List<IFrameUpdate>();
        _fixedFrameUpdates = new List<IFixedFrameUpdate>();
        _cleanupables = new List<ICleanupable>();
    }

    public void Add(IController controller)
    {
        if (controller is IInitializeable initializeable)
        {
            _initializeables.Add(initializeable);
        }
        if (controller is IFrameUpdate updateable)
        {
            _frameUpdates.Add(updateable);
        }
        if (controller is IFixedFrameUpdate fixedUpdateable)
        {
            _fixedFrameUpdates.Add(fixedUpdateable);
        }
        if (controller is ICleanupable cleanupable)
        {
            _cleanupables.Add(cleanupable);
        }
    }

    public void Initialize()
    {
        foreach (var initializeable in _initializeables)
        {
            initializeable.Initialize();
        }
    }

    public void FrameUpdate(float deltaTime)
    {
        foreach (var updateable in _frameUpdates)
        {
            updateable.FrameUpdate(deltaTime);
        }
    }

    public void FixedFrameUpdate(float deltaTime)
    {
        foreach (var fixedUpdate in _fixedFrameUpdates)
        {
            fixedUpdate.FixedFrameUpdate(deltaTime);
        }
    }

    public void Cleanup()
    {
        foreach (var cleanupable in _cleanupables)
        {
            cleanupable.Cleanup();
        }
    }
}