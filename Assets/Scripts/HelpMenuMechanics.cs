using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class HelpMenuMechanics : MonoBehaviour{
    public int currentIndex = 0;
    public int maxIndex = 5;
    [SerializeField] Button backButton = default;
    [SerializeField] Button forwardButton = default;
    public List<GameObject> groups = new List<GameObject>();
    [SerializeField] Button helpButton=default;
    [SerializeField] Image BG_Fade_IMG = default;
    public int counter=0;
    public float holdTime = 0f;
    [SerializeField] CreateBoard gameboard = default;
    

    void Start(){
        currentIndex = 5;
        maxIndex = 5;
        CheckLevelAndSetIndex();
        counter++;
    }

    private void CheckLevelAndSetIndex() {
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int level = LevelManager.levelManager.level;
        if (scroll == 0) {
            if (block == 1) {

                if (level == 1) {
                    maxIndex = 0;
                    currentIndex = 0;
                    StartCoroutine(HoldBeforeDropping(holdTime-0.75f));
                }
                if (level >= 2) {
                    currentIndex = 1;
                    maxIndex = 1;
                }
                if (level == 2) {
                    StartCoroutine(HoldBeforeDropping(holdTime));
                }

                if (level >= 7) {
                    currentIndex = 2;
                    maxIndex = 2;
                }
                if (level == 7) {
                    StartCoroutine(HoldBeforeDropping(holdTime));
                }


            }


            if (block == 2) {
                if (level == 1) {
                    currentIndex = 2;
                    maxIndex = 2;
                }
                if (level >= 2) {
                    currentIndex = 3;
                    maxIndex = 3;
                }
            }

            if (block == 3) {
                currentIndex = 4;
                maxIndex = 4;
                if (level == 1) {
                    StartCoroutine(HoldBeforeDropping(holdTime));
                }
            }

            if (block >= 4) {
                currentIndex = 5;
                maxIndex = 5;
                if (block == 4 && level == 1) {
                    StartCoroutine(HoldBeforeDropping(holdTime));
                }
            }
        }
    }
    


    public IEnumerator HoldBeforeDropping(float dropTime) {
        yield return new WaitForSeconds(dropTime);
        HelpButtonCommand();
    }

    private void SetCorrectGroup() {

        //All Help Menu Looping Images are child 0 except the 5th GRP which is child 1
        for (int i = 0; i < groups.Count; i++) {
            int childNumber_grp = 0;
            if (i == 5) {
                childNumber_grp = 1;
            }
            if (groups[i].gameObject.activeSelf == true) {
                groups[i].transform.GetChild(childNumber_grp).gameObject.GetComponent<LoopingImages>().StopCoroutine("SwitchImages");
                groups[i].gameObject.SetActive(false);
            }
        }

        groups[currentIndex].SetActive(true);
        int childNumber_current = 0;
        if (currentIndex == 5) {
            childNumber_current = 1;
        }
        groups[currentIndex].transform.GetChild(childNumber_current).gameObject.GetComponent<LoopingImages>().StartCoroutine("SwitchImages");
        SetUpArrowButtons();

    }
    
    private void SetUpArrowButtons() {
        if (currentIndex == 0) {
            backButton.gameObject.SetActive(false);
            //backButton.interactable = false;
            //backButton.GetComponent<Image>().raycastTarget = false;
        }
        else {
            backButton.gameObject.SetActive(true);
            //backButton.interactable = true;
            //backButton.GetComponent<Image>().raycastTarget = true;
        }

        if (currentIndex == maxIndex) {
            forwardButton.gameObject.SetActive(false);
            //forwardButton.interactable = false;
            //forwardButton.GetComponent<Image>().raycastTarget = false;
        }
        else {
            forwardButton.gameObject.SetActive(true);
            //forwardButton.interactable = true;
            //forwardButton.GetComponent<Image>().raycastTarget = true;
        }
    }

    public void ForwardButtonCommand() {
        PlayClickSound();
        if (currentIndex < maxIndex) {
            currentIndex++;
            SetCorrectGroup();
        }
    }

    public void ExitButtonCommand() {
        PlayClickSound();
        FindObjectOfType<CreateBoard>().EnableTouch();
        gameObject.GetComponent<Animator>().SetBool("DropHelpMenu", false);
        StartCoroutine(FadeTo(0 / 255f, 0.25f));

        CheckForTutorialLevelAndShowHints();
    }

    private void CheckForTutorialLevelAndShowHints() {
        int scroll = LevelManager.levelManager.scroll;
        int block = LevelManager.levelManager.block;
        int level = LevelManager.levelManager.level;

        if (scroll == 0) {
            if (block == 1) {
                if (level == 2 || level == 7) {
                    gameboard.ShowXsOnGameBoard(true);
                }

            }
            if (block == 4) {
                if (level == 1) {
                    gameboard.ShowXsOnGameBoard(true);
                }

            }

        }


    }

    private void PlayClickSound() {
        if (counter != 0) {
            //SoundManager.PlaySound("selectSFX1");
            FindObjectOfType<SoundManager>().PlaySound("selectSFX1");
        }
    }

    public void FirstSecret() {
        maxIndex = 3;
        currentIndex = 3;
        HelpButtonCommand();
    }

    public void HelpButtonCommand() {
        SetCorrectGroup();
        PlayClickSound();
        FindObjectOfType<CreateBoard>().DisableTouch();
        helpButton.interactable = false;
        gameObject.GetComponent<Animator>().SetBool("DropHelpMenu", true);
        BG_Fade_IMG.gameObject.SetActive(true);
        StartCoroutine(FadeTo(75 / 255f, 0.25f));
        counter++;
    }

    public void BackwardButtonCommand() {
        PlayClickSound();
        if (currentIndex > 0) {
            currentIndex--;
            SetCorrectGroup();
        }
    }

    IEnumerator FadeTo(float aValue, float aTime) {
        float alpha = BG_Fade_IMG.color.a;
        bool off = false;
        if (aValue == 0) {
            off = true;
        }
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, aValue, t));
            BG_Fade_IMG.color = newColor;
            yield return null;
        }

        if (off) {
            BG_Fade_IMG.gameObject.SetActive(false);
            helpButton.interactable = true;
        }
    }

}
