using UnityEngine;
using UnityEngine.Assertions;

public class BackgroundMove : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField, Range(0f, 0.0002f)] private float horizontalSpeed = 0.05f;
    [SerializeField, Range(0f, 0.0002f)] private float verticalSpeed = 0.05f;


    [Header("Bound")]
    [SerializeField, Range(0f, 1f)] private float verticalBound;
    [SerializeField, Range(0f, 1f)] private float horizontalBound;


    [Header("Grid Objects")]
    [SerializeField] private GameObject gridHorizontal;
    [SerializeField] private GameObject gridVertical;

    private Vector2 initialHPos;
    private Vector2 initialVPos;

    private void Start()
    {
        Assert.IsNotNull(gridHorizontal, "Background horizontal grid is not defined.");
        Assert.IsNotNull(gridVertical, "Background vertical grid is not defined.");

        initialHPos = Utils.WorldToViewport(gridHorizontal.transform.localPosition);
        initialVPos = Utils.WorldToViewport(gridVertical.transform.localPosition);
    }

    private void Update()
    {
        gridHorizontal.transform.localPosition = AddPosition(
            gridHorizontal.transform.localPosition,
            new(0, -verticalSpeed, 0)
        );
        gridVertical.transform.localPosition = AddPosition(
            gridVertical.transform.localPosition,
            new(horizontalSpeed, 0, 0)
        );

        if (Utils.WorldToViewport(gridHorizontal.transform.localPosition).y < 1)
            gridHorizontal.transform.localPosition = new(0, Utils.ViewportToWorld(initialHPos).y);

        if (Utils.WorldToViewport(gridVertical.transform.localPosition).x > 0)
            gridVertical.transform.localPosition = new(Utils.ViewportToWorld(initialVPos).x, 0);
    }

    private Vector3 AddPosition(Vector3 position, Vector3 addPosition)
    {
        return Utils.ViewportToWorld(Utils.WorldToViewport(position) + (Vector2)addPosition);
    }
}
