const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';
const RESEND_WAIT_TIME = 300;
const ImmediateActionAssignPerson = 'c8fdcac2-6357-46a6-8d6f-799517d10b17';
const carTypeNcr = 'f5908842-4e89-460a-804c-df931bae534e';
const variableTextFieldId = '4e4038ad-b575-4033-95a6-73686c8ad428';
const variableTextAreaFieldId = 'e160deb8-4c70-4319-8a3f-6ef6d2bb6a51';
const variablePAImplementorId = '8d888b30-5a1d-4d19-8d34-d75921b57742';
const variableCARApprovalAssignementId = 'b233d915-ea6d-4d5f-946f-16881e107562';
const sncrDispositionApprovalTypeId = 'c058c4ca-d731-4fb3-b2da-280031bcf843';
const sncrQurantineLogApprovalTypeId = 'af039e79-79ed-4fc2-8122-dd5214538feb';
const sncrVerifierTypeId = 'af039e79-79ed-4fc2-8122-dd5214538fea';
const variableScarTypeId = '322f7468-8708-4e12-9734-2bc7484cb2f8';
const scarTypeSncr = 'efa2e6ba-d34a-4f74-b44f-7ee885f115b7';

function generateUUID(): string {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    const r = (Math.random() * 16) | 0,
      v = c === 'x' ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}
export {
  EMPTY_GUID,
  RESEND_WAIT_TIME,
  ImmediateActionAssignPerson,
  carTypeNcr,
  generateUUID,
  variableTextFieldId,
  variableTextAreaFieldId,
  variablePAImplementorId,
  variableCARApprovalAssignementId,
  sncrDispositionApprovalTypeId,
  sncrQurantineLogApprovalTypeId,
  sncrVerifierTypeId,
  variableScarTypeId,
  scarTypeSncr,
};
