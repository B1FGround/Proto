using TMPro;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private enum BuildingType
    {
        Craft,
        DoorRim,
        Door,
        Inventory,
        Team,
        Roulette,
        None,
    }

    [SerializeField] private BuildingType buildingType = BuildingType.None;
    [SerializeField] private TMP_Text buildDesc;
    [SerializeField] private GameObject buildInfo;

    private GameObject player = null;
    private Material material = null;
    private Color outLineColor;
    public float speed = 1.0f;
    private float baseIntensity = 0.5f;
    private float maxIntensity = 1.5f;
    private float dissolveValue = 0f;
    private float dissolveSpeed = 1f; // 1초에 1씩 증가

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (buildingType == BuildingType.Door)
        {
            material = spriteRenderer.material;
            return;
        }

        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
            outLineColor = material.GetColor("_Color");
        }
        switch (buildingType)
        {
            case BuildingType.Craft:
                buildDesc.text = "Press 'E' to craft";
                break;

            case BuildingType.DoorRim:
                buildDesc.text = "Press 'E' to move";
                break;

            case BuildingType.Inventory:
                buildDesc.text = "Press 'E' to open inventory";
                break;

            case BuildingType.Team:
                buildDesc.text = "Press 'E' to open Team info";
                break;
            case BuildingType.Roulette:
                buildDesc.text = "Press 'E' to open Roulette";
                break;
            case BuildingType.None:
                break;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        if (Vector3.Distance(player.transform.position, transform.position) < 5f)
        {
            if (material == null)
                return;
            if (buildingType == BuildingType.Door)
            {
                dissolveValue = Mathf.Clamp01(dissolveValue + dissolveSpeed * Time.deltaTime);
                material.SetFloat("_Dissolve", dissolveValue);
                return;
            }
            float intensity = Mathf.PingPong(Time.time * speed, maxIntensity - baseIntensity) + baseIntensity;

            Color hdrColor = outLineColor * intensity;

            material.SetColor("_Color", hdrColor);

            if (buildInfo.activeSelf.Equals(false))
                buildInfo.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (buildingType)
                {
                    case BuildingType.Craft:
                        player.GetComponent<PlayerController>().OpenCraftUI();
                        break;

                    case BuildingType.DoorRim:
                        player.GetComponent<PlayerController>().SceneChange();
                        break;

                    case BuildingType.Inventory:
                        player.GetComponent<PlayerController>().OpenInventoryUI();
                        break;

                    case BuildingType.Team:
                        player.GetComponent<PlayerController>().OpenTeamUI();
                        break;
                    case BuildingType.Roulette:
                        UIManager.Instance.Open<RouletteUI>();
                        break;
                    case BuildingType.None:
                        break;
                }
            }
        }
        else
        {
            if (material == null)
                return;

            if (buildingType == BuildingType.Door)
            {
                dissolveValue = Mathf.Clamp01(dissolveValue - dissolveSpeed * Time.deltaTime);
                material.SetFloat("_Dissolve", dissolveValue);
                return;
            }

            material.SetColor("_Color", outLineColor * 0f);

            if (buildInfo.activeSelf.Equals(true))
                buildInfo.SetActive(false);
        }
    }
}