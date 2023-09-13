import { Injectable } from '@angular/core';
import * as forge from 'node-forge';
@Injectable({
  providedIn: 'root'
})
export class PasswordEncryptorService {

  private encryptionKey: string = 'VerySecretEncryptionKeyFromFront';
  encryptPassword(password: string): string {
    // const iv = CryptoJS.lib.WordArray.random(16);
    // const ciphertext = CryptoJS.AES.encrypt(password, this.encryptionKey, { iv }).toString();
    // return iv.toString() + ciphertext;
    return password;
  }
}
