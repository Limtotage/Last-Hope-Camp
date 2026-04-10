using UnityEngine;

public interface IPlacementStrategy
{
    void Handle(ResourcePlacementObjectsSO resourceData);
}
