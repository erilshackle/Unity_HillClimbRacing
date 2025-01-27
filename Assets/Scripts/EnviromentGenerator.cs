using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EndlessTerrainGenerator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;
    [SerializeField, Range(3f, 100f)] private int segmentLength = 50;
    [SerializeField, Range(1f, 50f)] private float xMultiplier = 2f;
    [SerializeField, Range(1f, 50f)] private float yMultiplier = 2f;
    [SerializeField, Range(0f, 1f)] private float curveSmoothness = 0.5f;
    [SerializeField] private float noiseStep = 6f;
    [SerializeField] private float bottom = 10f;
    [SerializeField] private Transform player; // Referência ao jogador
    [SerializeField] private float loadDistance = 100f; // Distância para carregar novos segmentos
    [SerializeField] private GameObject fuelPrefab; // Prefab de combustível
    [SerializeField] private float minFuelDistance = 10f; // Distância mínima entre combustíveis
    [SerializeField] private float maxFuelDistance = 20f; // Distância máxima entre combustíveis

    private float lastXPosition; // Para rastrear o último ponto gerado
    private List<Vector3> points = new List<Vector3>(); // Lista para armazenar os pontos do spline
    private float nextFuelXPosition = 0f; // Posição X para o próximo combustível

    void Start()
    {
        GenerateInitialSegment();
    }

    void Update()
    {
        // Gera novos segmentos conforme o jogador avança
        if (player.position.x > lastXPosition - loadDistance)
        {
            GenerateSegment();
        }
    }

    private void GenerateInitialSegment()
    {
        spriteShapeController.spline.Clear();
        points.Clear();

        for (int i = 0; i < segmentLength; i++)
        {
            Vector3 point = new Vector3(i * xMultiplier, Mathf.PerlinNoise(0, i * noiseStep) * yMultiplier);
            points.Add(point);
            spriteShapeController.spline.InsertPointAt(i, point);

            if (i != 0 && i != segmentLength - 1)
            {
                spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetLeftTangent(i, Vector3.left * xMultiplier * curveSmoothness);
                spriteShapeController.spline.SetRightTangent(i, Vector3.right * xMultiplier * curveSmoothness);
            }

            // Adiciona combustíveis
            TrySpawnFuel(point);
        }

        // Adiciona os pontos de fechamento para o terreno
        spriteShapeController.spline.InsertPointAt(segmentLength, new Vector3(points[points.Count - 1].x, transform.position.y - bottom));
        spriteShapeController.spline.InsertPointAt(segmentLength + 1, new Vector3(0, transform.position.y - bottom));

        lastXPosition = points[points.Count - 1].x;
        SetNextFuelPosition();
    }

    private void GenerateSegment()
    {
        int startIndex = points.Count;

        for (int i = 0; i < segmentLength; i++)
        {
            float x = lastXPosition + i * xMultiplier;
            float y = Mathf.PerlinNoise(0, (startIndex + i) * noiseStep) * yMultiplier;
            Vector3 point = new Vector3(x, y);

            points.Add(point);
            spriteShapeController.spline.InsertPointAt(spriteShapeController.spline.GetPointCount(), point);

            // Define tangentes
            int pointIndex = spriteShapeController.spline.GetPointCount() - 1;
            if (pointIndex > 0 && pointIndex < spriteShapeController.spline.GetPointCount() - 1)
            {
                spriteShapeController.spline.SetTangentMode(pointIndex, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetLeftTangent(pointIndex, Vector3.left * xMultiplier * curveSmoothness);
                spriteShapeController.spline.SetRightTangent(pointIndex, Vector3.right * xMultiplier * curveSmoothness);
            }

            TrySpawnFuel(point);
        }

        // Adiciona pontos inferiores para fechar o terreno
        spriteShapeController.spline.InsertPointAt(spriteShapeController.spline.GetPointCount(), new Vector3(points[points.Count - 1].x, transform.position.y - bottom));
        spriteShapeController.spline.InsertPointAt(spriteShapeController.spline.GetPointCount(), new Vector3(lastXPosition, transform.position.y - bottom));

        // Atualiza o último X
        lastXPosition = points[points.Count - 1].x;

        // Remove os segmentos antigos (opcional, para não gastar memória)
        if (points.Count > segmentLength * 3)
        {
            RemoveOldSegments();
        }
    }


    private void RemoveOldSegments()
    {
        int removeCount = segmentLength; // Número de pontos para remover
        points.RemoveRange(0, removeCount);

        spriteShapeController.spline.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            spriteShapeController.spline.InsertPointAt(i, points[i]);
        }
    }

    private void TrySpawnFuel(Vector3 point)
    {
        if (point.x >= nextFuelXPosition)
        {
            // Instancia o combustível
            Vector3 fuelPosition = new Vector3(point.x, point.y + 1.3f, 0); // Adiciona uma leve altura ao combustível
            Instantiate(fuelPrefab, fuelPosition, Quaternion.identity);

            // Define a posição do próximo combustível
            SetNextFuelPosition();
        }
    }

    private void SetNextFuelPosition()
    {
        nextFuelXPosition = lastXPosition + Random.Range(minFuelDistance, maxFuelDistance);
    }
}
