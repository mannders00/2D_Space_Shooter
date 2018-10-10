using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Transform wepSwitch;
    public Sprite[] images = new Sprite[6];

    int selectedWeapon = 5;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            shiftRight();   
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            shiftLeft();
        }
    }
    void Start() {
        updateWeaponUI();
	}

    public int getSelectedWeapon() {
        return selectedWeapon;
    }
    public void setSelectedWeapon(int selectedWeapon) {
        if ((selectedWeapon > 0) && (selectedWeapon < images.Length)) {
            this.selectedWeapon = selectedWeapon;
        }
        updateWeaponUI();
    }

    public void updateWeaponUI() {
        //Use GetComponent() a lot, hard to make concise because they are all different objects

        wepSwitch.GetChild(3).GetComponent<Image>().sprite = images[selectedWeapon]; //Middle (Selected)

        if(selectedWeapon > 1) {
            wepSwitch.GetChild(1).GetComponent<Image>().sprite = images[selectedWeapon - 1];
        } else {
            wepSwitch.GetChild(1).GetComponent<Image>().sprite = images[0];
        }

        if(selectedWeapon < (images.Length - 1)) {
            wepSwitch.GetChild(2).GetComponent<Image>().sprite = images[selectedWeapon + 1];
        } else {
            wepSwitch.GetChild(2).GetComponent<Image>().sprite = images[0];
        }
    }

    public void shiftLeft() {
        if (selectedWeapon > 1) {
            wepSwitch.GetChild(0).GetComponent<Image>().sprite = images[selectedWeapon - 2];
            GetComponent<Animator>().SetTrigger("ShiftRight");
            selectedWeapon--;
        }
    }

    public void shiftRight() {
        if (selectedWeapon < (images.Length - 1)) {

            if (selectedWeapon >= (images.Length - 2)) { //Remember that images[0] is the end
                wepSwitch.GetChild(0).GetComponent<Image>().sprite = images[0];
            } else {
                wepSwitch.GetChild(0).GetComponent<Image>().sprite = images[selectedWeapon + 2];
            }

            selectedWeapon++;
            GetComponent<Animator>().SetTrigger("ShiftLeft");
        }
    }

}
