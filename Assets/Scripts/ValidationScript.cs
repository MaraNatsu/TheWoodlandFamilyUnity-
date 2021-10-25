using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ValidationLibrary.Validators;
using FluentValidation;
using FluentValidation.Results;
using EFDataAccessLibrary.Entities;

public class ValidationScript : MonoBehaviour
{
    //Player player;
    //Room room;

    //public ValidationScript(Player player, Room room)
    //{
    //    this.player = player;
    //    this.room = room;
    //}

    public ValidationResult ValidateNickname(Player player)
    {
        PlayerValidator validator = new PlayerValidator();
        return validator.Validate(player);
    }

    public ValidationResult ValidateWordkey(Room room)
    {
        RoomValidator validator = new RoomValidator();
        return validator.Validate(room);
    }
}
