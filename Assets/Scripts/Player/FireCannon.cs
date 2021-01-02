using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FireCannon : MonoBehaviour
{
    public Animator animator;
    public GameObject muzzleFlash;
    public ParticleSystem emitter;
    public Text cannonReadout;
    public Text crosshair;
    private float lastAimedRange = 50.0f;
    private float maxAimRange = 500.0f; // avoids dot vanishing
    private float muzzleFlashDuration = 0.3f;
    public GameObject ShootingPoints;
    private AudioSource cannonShotSound;

    private int MAX_SHOTS = 4;
    private int shotsLeft = 4;
    [Range(1, 4)] public int startAmmoCount = 4;
    public GameObject[] ammoDisplayBoxes;
    public UnityEvent outOfAmmo = null;

    public Transform VRCamLens;

	public List<AudioClip> cannonSoundClips;

    private void AddArrayIntoListIfUnique(List<RaycastHit> toList, RaycastHit[] fromArray)
    {
        for (int i = 0; i < fromArray.Length; i++)
        {
            if (toList.Contains(fromArray[i]) == false)
            {
                toList.Add(fromArray[i]);
            }
        }

    }

    private void Start()
    {
        cannonShotSound = GetComponent<AudioSource>();
        crosshair.color = Color.cyan;

        shotsLeft = startAmmoCount;
        UpdateAmmoDisplay();
    }

    void UpdateAmmoDisplay() {
        for (int i = 0; i < ammoDisplayBoxes.Length; i++) {
            ammoDisplayBoxes[i].SetActive(i < shotsLeft);
            ammoDisplayBoxes[i].GetComponent<Animator>().SetTrigger("Blink");
        }
    }

    void FixedUpdate() {
        if(startedQuitTimerYet) {
            return; // avoid restarting coroutine timer
        }
        if (shotsLeft <= 0) {
            if (VRCamLens != null) {
                startedQuitTimerYet = VRCamLens.transform.localScale.x < 0.005f;
                VRCamLens.transform.localScale =
                    startedQuitTimerYet ? Vector3.zero :
                    new Vector3(VRCamLens.transform.localScale.x * 0.97f,
                        VRCamLens.transform.localScale.y,
                        VRCamLens.transform.localScale.z);
            } else {
                Quaternion targetTiltRot = transform.parent.rotation * Quaternion.AngleAxis(-55.0f, Vector3.right);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                           targetTiltRot, 0.07f);
                startedQuitTimerYet = Quaternion.Angle(transform.rotation, targetTiltRot) < 1.0f;
            }
            if(startedQuitTimerYet) {
                StartCoroutine(EndSoon());
            }
        }
    }

    bool startedQuitTimerYet = false;

    IEnumerator EndSoon() {
        yield return new WaitForSeconds(5.5f);
        SceneManager.LoadScene(0); // back to menu
    }

    bool fireHoldLock = false;

    void Update()
    {
        bool fireTriggerSqueezingNow = Input.GetAxis("TriggerAxis") > 0.2f;
        bool fireTriggerSqueezedFrame = (fireTriggerSqueezingNow && fireHoldLock==false);
        fireHoldLock = fireTriggerSqueezingNow;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0); // back to menu
        }
        if (lastAimedRange < 25.0f) {
            lastAimedRange = 25.0f;
        }
        crosshair.rectTransform.position = Camera.main.WorldToScreenPoint(transform.position + transform.forward * lastAimedRange);

        if (shotsLeft <= 0) {
            crosshair.color = Color.black;
            cannonReadout.text = "-";

            outOfAmmo?.Invoke();
            return;
        }

        float thicknessRange = 0.4f;
        RaycastHit[] laserMiddle = Physics.RaycastAll(transform.position, transform.forward);
        RaycastHit[] laserLeft = Physics.RaycastAll(transform.position-transform.right* thicknessRange, transform.forward);
        RaycastHit[] laserRight = Physics.RaycastAll(transform.position + transform.right * thicknessRange, transform.forward);
        RaycastHit[] laserUp = Physics.RaycastAll(transform.position+transform.up * thicknessRange, transform.forward);
        RaycastHit[] laserDown = Physics.RaycastAll(transform.position-transform.up * thicknessRange, transform.forward);
        float roundedPerc = 0.707f;
        RaycastHit[] laserUL = Physics.RaycastAll(transform.position + transform.up * thicknessRange * roundedPerc
                                                                     - transform.right * thicknessRange * roundedPerc, transform.forward);
        RaycastHit[] laserUR = Physics.RaycastAll(transform.position + transform.up * thicknessRange * roundedPerc
                                                                     + transform.right * thicknessRange * roundedPerc, transform.forward);
        RaycastHit[] laserDL = Physics.RaycastAll(transform.position - transform.up * thicknessRange * roundedPerc
                                                                     - transform.right * thicknessRange * roundedPerc, transform.forward);
        RaycastHit[] laserDR = Physics.RaycastAll(transform.position - transform.up * thicknessRange * roundedPerc
                                                                     + transform.right * thicknessRange * roundedPerc, transform.forward);

        List<RaycastHit> rhListTemp = new List<RaycastHit>();
        AddArrayIntoListIfUnique(rhListTemp, laserLeft);
        AddArrayIntoListIfUnique(rhListTemp, laserRight);
        AddArrayIntoListIfUnique(rhListTemp, laserUp);
        AddArrayIntoListIfUnique(rhListTemp, laserDown);
        AddArrayIntoListIfUnique(rhListTemp, laserUL);
        AddArrayIntoListIfUnique(rhListTemp, laserUR);
        AddArrayIntoListIfUnique(rhListTemp, laserDL);
        AddArrayIntoListIfUnique(rhListTemp, laserDR);
        AddArrayIntoListIfUnique(rhListTemp, laserMiddle);

        RaycastHit[] allRH = rhListTemp.ToArray(); // Physics.RaycastAll(transform.position, transform.forward);

        int hitsInRange = 0;
        bool didFire = false;
        crosshair.color = Color.red;
        for (int i=0; i< allRH.Length;i++)
        {
            RaycastHit rhInfo = allRH[i];

            crosshair.rectTransform.position = Camera.main.WorldToScreenPoint(rhInfo.point);
            lastAimedRange = Vector3.Distance(transform.position, rhInfo.point);

            ExplodeChainReact ecrScript = rhInfo.collider.gameObject.GetComponentInParent<ExplodeChainReact>();
            bool isItShootingPoint = (ShootingPoints ? rhInfo.collider.gameObject.transform.IsChildOf(ShootingPoints.transform) : false);
            if (Input.GetMouseButtonDown(0) || fireTriggerSqueezedFrame)
            {
                if (isItShootingPoint)
                {
                    transform.root.position = rhInfo.collider.transform.position;
                    transform.root.rotation = rhInfo.collider.transform.rotation;
                    break; // shooting point, bail out
                }
                else
                {
                    didFire = true; // setting only once, to not repeat per sub raycast
                    /*GameObject testBlastVFX = Resources.Load("Explosion5m") as GameObject;
                    GameObject pointGO = GameObject.Instantiate(testBlastVFX, transform.position + transform.forward * 7.0f,
                        transform.rotation);*/

                    Invoke(nameof(DisableMuzzleFlash), muzzleFlashDuration);

                    // Debug.Log("DIRECT HIT:" + rhInfo.collider.gameObject.name);
                    if (ecrScript)
                    {
                        ecrScript.Explode(1);
                    }
                }
            }
            else
            {
                if (ecrScript)
                {
                    hitsInRange += ecrScript.HitsInRange();
                } else if (isItShootingPoint)
                {
                    crosshair.color = Color.green;
                }
                else
                {
                    cannonReadout.text = "X";
                }
            }
        }

        if(didFire) {
            shotsLeft--;
            UpdateAmmoDisplay();
			cannonShotSound.clip = cannonSoundClips[Random.Range(0, cannonSoundClips.Count)];
            cannonShotSound.Play();
            emitter.Emit(1000);
            animator.SetTrigger("Fire");
            muzzleFlash.SetActive(true);
            if (SceneManager.GetActiveScene().name == "ChaosDimension")
            {
                Debug.Log("shot fired in Chaos Dimension");
                TimeKeeper.instance.RandomJoltTimeOffset();
            }
            TimeKeeper.instance.fakeTimePace = 1.0f;
        }

        if (lastAimedRange> maxAimRange)
        {
            lastAimedRange = maxAimRange;
        }

        if (allRH.Length==0) // essentially, else / nothing under gun
        {
            crosshair.color = Color.red;

            cannonReadout.text = "0";
        } else if(hitsInRange != 0)
        {
            crosshair.color = Color.cyan;
            cannonReadout.text = "" + hitsInRange;
        }
        else
        {
            crosshair.color = Color.black;
            cannonReadout.text = "X";
        }
    }

    private void DisableMuzzleFlash()
    {
        muzzleFlash.SetActive( false );
    }
}
